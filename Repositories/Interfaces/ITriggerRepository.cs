using FlowForge.Models.Entities;

namespace FlowForge.Repositories.Interfaces;

public interface ITriggerRepository
{
    Task<List<Trigger>> GetTriggersByWorkflowIdAsync(int workflowId);
    Task<Trigger?> GetTriggerByIdAsync(int workflowId, int id);
    Task<Trigger> CreateTriggerAsync(Trigger trigger);
    Task<Trigger> UpdateTriggerAsync(Trigger trigger);
    Task<bool> DeleteTriggerAsync(Trigger trigger);
}
