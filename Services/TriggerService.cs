using FlowForge.Models.DTOs;
using FlowForge.Models.Entities;
using FlowForge.Repositories.Interfaces;
using FlowForge.Services.Interfaces;

namespace FlowForge.Services;

public class TriggerService : ITriggerService
{
    private readonly IWorkflowRepository _workflowRepository;
    private readonly ITriggerRepository _triggerRepository;

    public TriggerService(IWorkflowRepository workflowRepository, ITriggerRepository triggerRepository)
    {
        _workflowRepository = workflowRepository;
        _triggerRepository = triggerRepository;
    }

    public async Task<bool> WorkflowExistsAsync(int workflowId)
    {
        return await _workflowRepository.GetWorkflowByIdAsync(workflowId) is not null;
    }

    public Task<List<Trigger>> GetTriggersByWorkflowIdAsync(int workflowId)
    {
        return _triggerRepository.GetTriggersByWorkflowIdAsync(workflowId);
    }

    public Task<Trigger?> GetTriggerByIdAsync(int workflowId, int id)
    {
        return _triggerRepository.GetTriggerByIdAsync(workflowId, id);
    }

    public Task<Trigger> CreateTriggerAsync(int workflowId, CreateTriggerDto triggerDto)
    {
        var trigger = new Trigger(workflowId, triggerDto.ConfigJson, triggerDto.TriggerType);
        return _triggerRepository.CreateTriggerAsync(trigger);
    }

    public async Task<Trigger?> UpdateTriggerAsync(int workflowId, int id, UpdateTriggerDto triggerDto)
    {
        var trigger = await _triggerRepository.GetTriggerByIdAsync(workflowId, id);
        if (trigger is null)
        {
            return null;
        }

        trigger.TriggerType = triggerDto.TriggerType;
        trigger.ConfigJson = triggerDto.ConfigJson;

        return await _triggerRepository.UpdateTriggerAsync(trigger);
    }

    public async Task<bool> DeleteTriggerAsync(int workflowId, int id)
    {
        var trigger = await _triggerRepository.GetTriggerByIdAsync(workflowId, id);
        return trigger is not null && await _triggerRepository.DeleteTriggerAsync(trigger);
    }
}
