using FlowForge.Models.Enums;

namespace FlowForge.Models.Entities;

public class WorkflowExecution 
{
    public int Id { get; set; }
    public int WorkflowId { get; set; }
    public TriggerType TriggerType { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public ExecutionStatus Status { get; set; }

    public Workflow Workflow { get; set; } = null!;
    public ICollection<StepExecutionLog> StepExecutionLogs { get; set; } = new List<StepExecutionLog>();

    public WorkflowExecution(int workflowId, TriggerType triggerType)
    {
        WorkflowId = workflowId;
        TriggerType = triggerType;
        Status = ExecutionStatus.Running;
        StartedAt = DateTime.UtcNow;
    }
}