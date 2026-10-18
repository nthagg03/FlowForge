using FlowForge.Models.Enums;

namespace FlowForge.Models.Entities;

public class Step
{
    public int Id { get; set; }
    public int WorkflowId { get; set; }
    public int Order { get; set; } 
    public ActionType ActionType { get; set; }
    public string ConfigJson { get; set; } 
    public Workflow Workflow { get; set; } = null!;

    public Step(int workflowId, int order, string configJson, ActionType actionType)
    {
        WorkflowId = workflowId;
        Order = order;
        ActionType = actionType;
        ConfigJson = configJson;
    }
}
