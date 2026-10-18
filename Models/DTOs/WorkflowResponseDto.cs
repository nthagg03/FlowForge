using FlowForge.Models.Enums;

namespace FlowForge.Models.DTOs;

public class WorkflowResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public WorkflowStatus Status { get; set; }
    public required string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}
