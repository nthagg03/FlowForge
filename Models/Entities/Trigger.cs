using FlowForge.Models.Enums;

namespace FlowForge.Models.Entities;


public class Trigger
{
    public int Id { get; set; }
    public int WorkflowId { get; set; }
    public string ConfigJson { get; set; } 
    public TriggerType TriggerType { get; set; }

    public Trigger(int workflowId, string configJson, TriggerType triggerType)
    {
        WorkflowId = workflowId;
        ConfigJson = configJson;
        TriggerType = triggerType;
    }
}