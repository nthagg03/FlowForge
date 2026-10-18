using FlowForge.Models.Enums;

namespace FlowForge.Models.DTOs;

public class CreateTriggerDto
{
    public TriggerType TriggerType { get; set; }
    public required string ConfigJson { get; set; }
}
