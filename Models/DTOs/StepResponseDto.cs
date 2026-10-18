using FlowForge.Models.Enums;

namespace FlowForge.Models.DTOs;

public class StepResponseDto
{
    public int Id { get; set; }
    public int WorkflowId { get; set; }
    public int Order { get; set; }
    public ActionType ActionType { get; set; }
    public required string ConfigJson { get; set; }
}
