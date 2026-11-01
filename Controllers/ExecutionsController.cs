using FlowForge.Exceptions;
using FlowForge.Models.DTOs;
using FlowForge.Repositories.Interfaces;
using FlowForge.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowForge.Controllers;

[ApiController]
public class ExecutionsController : ControllerBase
{
    private const int MaxPageSize = 100;

    private readonly IExecutionRepository _executionRepository;
    private readonly IWorkflowService _workflowService;

    public ExecutionsController(
        IExecutionRepository executionRepository,
        IWorkflowService workflowService)
    {
        _executionRepository = executionRepository;
        _workflowService = workflowService;
    }

    /// <summary>
    /// GET /api/workflows/{workflowId}/executions
    /// Returns paginated executions for a workflow, newest first.
    /// 404 if the workflow does not exist.
    /// </summary>
    [HttpGet("api/workflows/{workflowId:int}/executions")]
    public async Task<ActionResult<IEnumerable<ExecutionResponseDto>>> GetExecutionsByWorkflow(
        int workflowId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var workflow = await _workflowService.GetWorkflowByIdAsync(workflowId);
        if (workflow is null)
        {
            throw new NotFoundException($"Workflow {workflowId} not found.");
        }

        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);
        page = Math.Max(page, 1);

        var executions = await _executionRepository.GetExecutionsByWorkflowIdAsync(
            workflowId, page, pageSize);

        return Ok(executions.Select(e => new ExecutionResponseDto
        {
            Id = e.Id,
            WorkflowId = e.WorkflowId,
            TriggerType = e.TriggerType,
            Status = e.Status,
            StartedAt = e.StartedAt,
            CompletedAt = e.CompletedAt
        }));
    }

    /// <summary>
    /// GET /api/executions/{id}
    /// Returns execution detail with ordered step logs.
    /// 404 if the execution does not exist.
    /// </summary>
    [HttpGet("api/executions/{id:int}")]
    public async Task<ActionResult<ExecutionDetailDto>> GetExecutionById(int id)
    {
        var execution = await _executionRepository.GetExecutionByIdAsync(id);
        if (execution is null)
        {
            throw new NotFoundException($"Execution {id} not found.");
        }

        var dto = new ExecutionDetailDto
        {
            Id = execution.Id,
            WorkflowId = execution.WorkflowId,
            TriggerType = execution.TriggerType,
            Status = execution.Status,
            StartedAt = execution.StartedAt,
            CompletedAt = execution.CompletedAt,
            StepLogs = execution.StepExecutionLogs.Select(l => new StepExecutionLogResponseDto
            {
                Id = l.Id,
                StepId = l.StepId,
                Order = l.Step.Order,
                ActionType = l.Step.ActionType,
                Status = l.Status,
                Output = l.Output,
                ErrorMessage = l.ErrorMessage,
                AttemptCount = l.AttemptCount,
                StartedAt = l.StartedAt,
                CompletedAt = l.CompletedAt
            }).ToList()
        };

        return Ok(dto);
    }
}
