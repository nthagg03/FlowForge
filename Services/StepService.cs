using FlowForge.Models.DTOs;
using FlowForge.Models.Entities;
using FlowForge.Repositories.Interfaces;
using FlowForge.Services.Interfaces;

namespace FlowForge.Services;

public class StepService : IStepService
{
    private readonly IWorkflowRepository _workflowRepository;
    private readonly IStepRepository _stepRepository;

    public StepService(IWorkflowRepository workflowRepository, IStepRepository stepRepository)
    {
        _workflowRepository = workflowRepository;
        _stepRepository = stepRepository;
    }

    public async Task<bool> WorkflowExistsAsync(int workflowId)
    {
        return await _workflowRepository.GetWorkflowByIdAsync(workflowId) is not null;
    }

    public Task<List<Step>> GetStepsByWorkflowIdAsync(int workflowId)
    {
        return _stepRepository.GetStepsByWorkflowIdAsync(workflowId);
    }

    public Task<Step?> GetStepByIdAsync(int workflowId, int id)
    {
        return _stepRepository.GetStepByIdAsync(workflowId, id);
    }

    public Task<Step> CreateStepAsync(int workflowId, CreateStepDto stepDto)
    {
        var step = new Step(workflowId, stepDto.Order, stepDto.ConfigJson, stepDto.ActionType);
        return _stepRepository.CreateStepAsync(step);
    }

    public async Task<Step?> UpdateStepAsync(int workflowId, int id, UpdateStepDto stepDto)
    {
        var step = await _stepRepository.GetStepByIdAsync(workflowId, id);
        if (step is null)
        {
            return null;
        }

        step.Order = stepDto.Order;
        step.ActionType = stepDto.ActionType;
        step.ConfigJson = stepDto.ConfigJson;

        return await _stepRepository.UpdateStepAsync(step);
    }

    public async Task<bool> DeleteStepAsync(int workflowId, int id)
    {
        var step = await _stepRepository.GetStepByIdAsync(workflowId, id);
        return step is not null && await _stepRepository.DeleteStepAsync(step);
    }

    public Task<bool> IsOrderInUseAsync(int workflowId, int order, int? excludedStepId = null)
    {
        return _stepRepository.IsOrderInUseAsync(workflowId, order, excludedStepId);
    }
}
