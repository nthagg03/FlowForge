using FlowForge.Models.Entities;

namespace FlowForge.Repositories.Interfaces;

public interface IWorkflowRepository
{
    Task<Workflow> CreateWorkflowAsync(Workflow workflow);
    Task<Workflow?> GetWorkflowByIdAsync(int id);
    Task<List<Workflow>> GetAllWorkflowsAsync();
    Task<Workflow?> UpdateWorkflowAsync( Workflow workflow);
    Task<bool> DeleteWorkflowAsync(int id);
}