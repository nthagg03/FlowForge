using FlowForge.Models.Enums;

namespace FlowForge.Models.Entities;

public class Workflow
{
    public int Id { get; set;}
    public string Name { get; set; }
    public WorkflowStatus Status { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Step> Steps { get; set; } = new List<Step>();
    public ICollection<Trigger> Triggers { get; set; } = new List<Trigger>();

    public Workflow(string name, string createdBy)
    {
        Name = name;
        CreatedBy = createdBy;
        Status = WorkflowStatus.Draft;
        CreatedAt = DateTime.Now;
    }
}
