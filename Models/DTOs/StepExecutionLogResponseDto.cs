using FlowForge.Models.Enums;

namespace FlowForge.Models.DTOs;

public class StepExecutionLogResponseDto
{
    public int Id { get; set; }
    public int StepId { get; set; }
    public int Order { get; set; }
    public ActionType ActionType { get; set; }
    public ExecutionStatus Status { get; set; }
    public string? Output { get; set; }
    public string? ErrorMessage { get; set; }
    public int AttemptCount { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
