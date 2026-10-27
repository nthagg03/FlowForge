using FlowForge.Models.Enums;

namespace FlowForge.Models.Entities;

public class StepExecutionLog{
    public int Id { get; set; }
    public int WorkflowExecutionId { get; set; }
    public int StepId { get; set; }
    public ExecutionStatus Status { get; set; }
    public string? Output { get; set; }
    public string? ErrorMessage { get; set; }
    public int AttemptCount { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    public WorkflowExecution WorkflowExecution { get; set; } = null!;
    public Step Step { get; set; } = null!;

    public StepExecutionLog(int workflowExecutionId, int stepId)
    {
        WorkflowExecutionId = workflowExecutionId;
        StepId = stepId;
        Status = ExecutionStatus.Running;
        StartedAt = DateTime.UtcNow;
    }
}
