using FlowForge.Models.Enums;

namespace FlowForge.Models.DTOs;

public class UpdateTriggerDto
{
    public TriggerType TriggerType { get; set; }
    public required string ConfigJson { get; set; }
}
