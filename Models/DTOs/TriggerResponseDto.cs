using FlowForge.Models.Enums;

namespace FlowForge.Models.DTOs;

public class TriggerResponseDto
{
    public int Id { get; set; }
    public int WorkflowId { get; set; }
    public TriggerType TriggerType { get; set; }
    public required string ConfigJson { get; set; }
}
