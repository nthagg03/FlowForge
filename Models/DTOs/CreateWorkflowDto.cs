using FlowForge.Models.Enums;

namespace FlowForge.Models.DTOs;

public class CreateWorkflowDto
{
    public required string Name { get; set; }
    public required string CreatedBy { get; set; }


}