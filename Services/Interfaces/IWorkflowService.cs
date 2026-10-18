using FlowForge.Models.Entities;
using FlowForge.Models.DTOs;

namespace FlowForge.Services.Interfaces;

public interface IWorkflowService
{
    Task<Workflow> CreateWorkflowAsync(CreateWorkflowDto workflowDto);
    Task<Workflow?> GetWorkflowByIdAsync(int id);
    Task<List<Workflow>> GetAllWorkflowsAsync();
    Task<Workflow?> UpdateWorkflowAsync(int id, UpdateWorkflowDto workflowDto);
    Task<bool> DeleteWorkflowAsync(int id);
}