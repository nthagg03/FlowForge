using FlowForge.Models.Entities;
using FlowForge.Models.Enums;

namespace FlowForge.Services.ExecutionEngine;

public interface IWorkflowExecutor
{
    /// <summary>
    /// Runs all steps of the specified workflow synchronously and returns the completed execution.
    /// Throws NotFoundException, ConflictException, or BadRequestException for business rule violations.
    /// Never throws for step-level failures — those are recorded in StepExecutionLog.
    /// </summary>
    Task<WorkflowExecution> ExecuteAsync(int workflowId, TriggerType triggerType);
}
