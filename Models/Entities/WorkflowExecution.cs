using FlowForge.Models.Enums;

namespace FlowForge.Models.Entities;

public class WorkflowExecution 
{
    public int Id { get; set; }
    public int WorkflowId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public ExecutionStatus Status {get; set;}

    public WorkflowExecution(int workflowId)
    {
        WorkflowId = workflowId;
        Status = ExecutionStatus.Running;
        StartedAt = DateTime.Now;
    }

}