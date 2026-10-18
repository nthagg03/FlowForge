using FlowForge.Models.Enums;

namespace FlowForge.Models.DTOs;

public class UpdateStepDto
{
    public int Order { get; set; }
    public ActionType ActionType { get; set; }
    public required string ConfigJson { get; set; }
}
