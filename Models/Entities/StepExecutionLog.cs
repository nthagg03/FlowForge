using FlowForge.Models.Enums;

namespace FlowForge.Models.Entities;

public class StepExecutionLog{
    public int Id { get; set; }
    public int WorkflowExecutionId { get; set; }
    public int StepId { get; set; }
    public ExecutionStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    public StepExecutionLog(int workflowExecutionId, int stepId, ExecutionStatus status, string? errorMessage)
    {
        WorkflowExecutionId = workflowExecutionId;
        StepId = stepId;
        Status = status;
        ErrorMessage = errorMessage;
        StartedAt = DateTime.Now;
    }
}
