using FlowForge.Models.DTOs;
using FlowForge.Models.Entities;

namespace FlowForge.Services.Interfaces;

public interface ITriggerService
{
    Task<bool> WorkflowExistsAsync(int workflowId);
    Task<List<Trigger>> GetTriggersByWorkflowIdAsync(int workflowId);
    Task<Trigger?> GetTriggerByIdAsync(int workflowId, int id);
    Task<Trigger> CreateTriggerAsync(int workflowId, CreateTriggerDto triggerDto);
    Task<Trigger?> UpdateTriggerAsync(int workflowId, int id, UpdateTriggerDto triggerDto);
    Task<bool> DeleteTriggerAsync(int workflowId, int id);
}
