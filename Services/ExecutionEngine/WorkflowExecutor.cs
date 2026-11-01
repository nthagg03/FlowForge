using FlowForge.Exceptions;
using FlowForge.Models.Entities;
using FlowForge.Models.Enums;
using FlowForge.Repositories.Interfaces;

namespace FlowForge.Services.ExecutionEngine;

public class WorkflowExecutor : IWorkflowExecutor
{
    // Maximum total time budget for one workflow run (guards against handler hangs).
    private const int ExecutionTimeoutSeconds = 60;

    // Maximum length of step output stored to the database (matches DB column constraint).
    private const int MaxOutputLength = 2000;

    // Delay between retry attempts in milliseconds.
    private static readonly int[] RetryDelaysMs = [1000, 2000];

    private readonly IWorkflowRepository _workflowRepository;
    private readonly IExecutionRepository _executionRepository;
    private readonly IActionHandlerResolver _handlerResolver;
    private readonly ILogger<WorkflowExecutor> _logger;
    private readonly TimeProvider _timeProvider;

    public WorkflowExecutor(
        IWorkflowRepository workflowRepository,
        IExecutionRepository executionRepository,
        IActionHandlerResolver handlerResolver,
        ILogger<WorkflowExecutor> logger,
        TimeProvider timeProvider)
    {
        _workflowRepository = workflowRepository;
        _executionRepository = executionRepository;
        _handlerResolver = handlerResolver;
        _logger = logger;
        _timeProvider = timeProvider;
    }

    public async Task<WorkflowExecution> ExecuteAsync(int workflowId, TriggerType triggerType)
    {
        // --- Pre-flight checks --------------------------------------------------
        var workflow = await _workflowRepository.GetWorkflowWithStepsAsync(workflowId)
            ?? throw new NotFoundException($"Workflow {workflowId} not found.");

        if (workflow.Status != WorkflowStatus.Active)
        {
            throw new ConflictException(
                $"Workflow '{workflow.Name}' is not Active (current status: {workflow.Status}).");
        }

        var steps = workflow.Steps.OrderBy(s => s.Order).ToList();
        if (steps.Count == 0)
        {
            throw new BadRequestException(
                $"Workflow '{workflow.Name}' has no steps to execute.");
        }

        // --- Create execution record --------------------------------------------
        var execution = await _executionRepository.CreateExecutionAsync(
            new WorkflowExecution(workflowId, triggerType));

        // Use a dedicated CancellationToken — never pass HttpContext.RequestAborted.
        using var cts = new CancellationTokenSource(
            TimeSpan.FromSeconds(ExecutionTimeoutSeconds));
        var ct = cts.Token;

        var overallSuccess = true;

        try
        {
            foreach (var step in steps)
            {
                var stepLog = await _executionRepository.CreateStepLogAsync(
                    new StepExecutionLog(execution.Id, step.Id));

                var stepSuccess = await ExecuteStepAsync(step, stepLog, ct);

                if (!stepSuccess)
                {
                    overallSuccess = false;
                    break; // stop; remaining steps are not run and have no log
                }
            }
        }
        finally
        {
            // Always finalise the execution — no execution is left in Running state.
            execution.Status = overallSuccess ? ExecutionStatus.Success : ExecutionStatus.Failed;
            execution.CompletedAt = _timeProvider.GetUtcNow().UtcDateTime;
            await SafeSaveAsync(execution);
        }

        return execution;
    }

    // -------------------------------------------------------------------------

    /// <summary>Returns true if the step succeeded; false if it failed after all retries.</summary>
    private async Task<bool> ExecuteStepAsync(
        Step step,
        StepExecutionLog log,
        CancellationToken ct)
    {
        const int maxAttempts = 3;
        ActionExecutionResult? lastResult = null;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            log.AttemptCount = attempt;

            try
            {
                IActionHandler handler;
                try
                {
                    handler = _handlerResolver.GetHandler(step.ActionType);
                }
                catch (NotSupportedException ex)
                {
                    // No handler registered — treat as non-retryable failure.
                    lastResult = ActionExecutionResult.Failed(ex.Message, isRetryable: false);
                    break;
                }

                lastResult = await handler.ExecuteAsync(step.ConfigJson, ct);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                // Unexpected exception from handler — non-retryable, do not propagate.
                lastResult = ActionExecutionResult.Failed(
                    $"Unhandled exception in handler: {ex.GetType().Name}: {ex.Message}",
                    isRetryable: false);
                break;
            }

            if (lastResult.Success)
            {
                break;
            }

            if (!lastResult.IsRetryable || attempt == maxAttempts)
            {
                break;
            }

            // Wait before next retry; respect cancellation.
            var delayIndex = attempt - 1; // attempt 1 → delay[0], attempt 2 → delay[1]
            if (delayIndex < RetryDelaysMs.Length)
            {
                await Task.Delay(RetryDelaysMs[delayIndex], ct);
            }
        }

        // Commit final state of the log immediately.
        if (lastResult is { Success: true })
        {
            log.Status = ExecutionStatus.Success;
            log.Output = Truncate(lastResult.Output);
        }
        else
        {
            log.Status = ExecutionStatus.Failed;
            log.ErrorMessage = lastResult?.ErrorMessage;
        }

        log.CompletedAt = _timeProvider.GetUtcNow().UtcDateTime;
        await SafeSaveAsync(log);

        return log.Status == ExecutionStatus.Success;
    }

    // -------------------------------------------------------------------------

    private string? Truncate(string? value) =>
        value is null || value.Length <= MaxOutputLength
            ? value
            : value[..MaxOutputLength];

    /// <summary>
    /// Best-effort save that logs but does not rethrow, so the finally block in
    /// ExecuteAsync always completes even when a DB error occurs mid-execution.
    /// </summary>
    private async Task SafeSaveAsync(object context)
    {
        try
        {
            await _executionRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to persist execution state for {Context}", context.GetType().Name);
        }
    }
}
