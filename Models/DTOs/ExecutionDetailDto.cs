using FlowForge.Models.Enums;

namespace FlowForge.Models.DTOs;

public class ExecutionDetailDto
{
    public int Id { get; set; }
    public int WorkflowId { get; set; }
    public TriggerType TriggerType { get; set; }
    public ExecutionStatus Status { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public List<StepExecutionLogResponseDto> StepLogs { get; set; } = [];
}
