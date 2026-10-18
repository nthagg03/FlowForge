using FlowForge.Models.DTOs;
using FlowForge.Models.Entities;
using FlowForge.Models.Enums;
using FlowForge.Services;
using FlowForge.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowForge.Controllers;

[ApiController]
[Route("api/workflows/{workflowId:int}/triggers")]
public class TriggersController : ControllerBase
{
    private readonly ITriggerService _triggerService;

    public TriggersController(ITriggerService triggerService)
    {
        _triggerService = triggerService;
    }

    [HttpPost]
    public async Task<ActionResult<TriggerResponseDto>> CreateTrigger(int workflowId, CreateTriggerDto triggerDto)
    {
        if (!await _triggerService.WorkflowExistsAsync(workflowId))
        {
            return NotFound();
        }

        if (!TryValidate(triggerDto.TriggerType, triggerDto.ConfigJson, out var validationResult))
        {
            return validationResult;
        }

        var trigger = await _triggerService.CreateTriggerAsync(workflowId, triggerDto);
        return CreatedAtAction(nameof(GetTriggerById), new { workflowId, id = trigger.Id }, ToResponseDto(trigger));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TriggerResponseDto>>> GetAllTriggers(int workflowId)
    {
        if (!await _triggerService.WorkflowExistsAsync(workflowId))
        {
            return NotFound();
        }

        var triggers = await _triggerService.GetTriggersByWorkflowIdAsync(workflowId);
        return Ok(triggers.Select(ToResponseDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TriggerResponseDto>> GetTriggerById(int workflowId, int id)
    {
        var trigger = await _triggerService.GetTriggerByIdAsync(workflowId, id);
        return trigger is null ? NotFound() : Ok(ToResponseDto(trigger));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TriggerResponseDto>> UpdateTrigger(int workflowId, int id, UpdateTriggerDto triggerDto)
    {
        if (!await _triggerService.WorkflowExistsAsync(workflowId))
        {
            return NotFound();
        }

        if (!TryValidate(triggerDto.TriggerType, triggerDto.ConfigJson, out var validationResult))
        {
            return validationResult;
        }

        var trigger = await _triggerService.UpdateTriggerAsync(workflowId, id, triggerDto);
        return trigger is null ? NotFound() : Ok(ToResponseDto(trigger));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTrigger(int workflowId, int id)
    {
        if (!await _triggerService.WorkflowExistsAsync(workflowId))
        {
            return NotFound();
        }

        return await _triggerService.DeleteTriggerAsync(workflowId, id) ? NoContent() : NotFound();
    }

    private bool TryValidate(TriggerType triggerType, string configJson, out ActionResult validationResult)
    {
        if (!ConfigurationJsonValidator.TryValidateTrigger(triggerType, configJson, out var configError))
        {
            ModelState.AddModelError("configJson", configError);
        }

        validationResult = ValidationProblem(ModelState);
        return ModelState.IsValid;
    }

    private static TriggerResponseDto ToResponseDto(Trigger trigger)
    {
        return new TriggerResponseDto
        {
            Id = trigger.Id,
            WorkflowId = trigger.WorkflowId,
            TriggerType = trigger.TriggerType,
            ConfigJson = trigger.ConfigJson
        };
    }
}
