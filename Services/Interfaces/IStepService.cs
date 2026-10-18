using FlowForge.Models.DTOs;
using FlowForge.Models.Entities;

namespace FlowForge.Services.Interfaces;

public interface IStepService
{
    Task<bool> WorkflowExistsAsync(int workflowId);
    Task<List<Step>> GetStepsByWorkflowIdAsync(int workflowId);
    Task<Step?> GetStepByIdAsync(int workflowId, int id);
    Task<Step> CreateStepAsync(int workflowId, CreateStepDto stepDto);
    Task<Step?> UpdateStepAsync(int workflowId, int id, UpdateStepDto stepDto);
    Task<bool> DeleteStepAsync(int workflowId, int id);
    Task<bool> IsOrderInUseAsync(int workflowId, int order, int? excludedStepId = null);
}
