using FlowForge.Models.Entities;
using FlowForge.Models.DTOs;
using FlowForge.Repositories.Interfaces;
using FlowForge.Services.Interfaces;

namespace FlowForge.Services;

public class WorkflowService : IWorkflowService
{
    private readonly IWorkflowRepository _workflowRepository;

    public WorkflowService(IWorkflowRepository workflowRepository)
    {
        _workflowRepository = workflowRepository;
    }

    public async Task<Workflow> CreateWorkflowAsync(CreateWorkflowDto workflowDto)
    {
        var workflow = new Workflow(workflowDto.Name, workflowDto.CreatedBy);
        return await _workflowRepository.CreateWorkflowAsync(workflow);
    }

    public async Task<Workflow?> GetWorkflowByIdAsync(int id)
    {
        return await _workflowRepository.GetWorkflowByIdAsync(id);
    }

    public async Task<List<Workflow>> GetAllWorkflowsAsync()
    {
        return await _workflowRepository.GetAllWorkflowsAsync();
    }

    public async Task<Workflow?> UpdateWorkflowAsync(int id, UpdateWorkflowDto workflowDto)
    {
        var workflow = new Workflow(workflowDto.Name, string.Empty)
        {
            Id = id,
            Status = workflowDto.Status
        };
        return await _workflowRepository.UpdateWorkflowAsync(workflow);
    }

    public async Task<bool> DeleteWorkflowAsync(int id)
    {
        return await _workflowRepository.DeleteWorkflowAsync(id);
    }
}