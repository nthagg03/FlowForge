using FlowForge.Models.Enums;

namespace FlowForge.Models.DTOs;

public class UpdateWorkflowDto
{
    public required string Name { get; set; }
    public WorkflowStatus Status { get; set; }


}