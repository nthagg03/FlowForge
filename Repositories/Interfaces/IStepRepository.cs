using FlowForge.Models.Entities;

namespace FlowForge.Repositories.Interfaces;

public interface IStepRepository
{
    Task<List<Step>> GetStepsByWorkflowIdAsync(int workflowId);
    Task<Step?> GetStepByIdAsync(int workflowId, int id);
    Task<Step> CreateStepAsync(Step step);
    Task<Step> UpdateStepAsync(Step step);
    Task<bool> DeleteStepAsync(Step step);
    Task<bool> IsOrderInUseAsync(int workflowId, int order, int? excludedStepId = null);
}
