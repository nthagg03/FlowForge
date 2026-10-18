using FlowForge.Models.DTOs;
using FlowForge.Models.Entities;
using FlowForge.Models.Enums;
using FlowForge.Services;
using FlowForge.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowForge.Controllers;

[ApiController]
[Route("api/workflows/{workflowId:int}/steps")]
public class StepsController : ControllerBase
{
    private readonly IStepService _stepService;

    public StepsController(IStepService stepService)
    {
        _stepService = stepService;
    }

    [HttpPost]
    public async Task<ActionResult<StepResponseDto>> CreateStep(int workflowId, CreateStepDto stepDto)
    {
        if (!await _stepService.WorkflowExistsAsync(workflowId))
        {
            return NotFound();
        }

        if (!TryValidate(stepDto.Order, stepDto.ActionType, stepDto.ConfigJson, out var validationResult))
        {
            return validationResult;
        }

        if (await _stepService.IsOrderInUseAsync(workflowId, stepDto.Order))
        {
            return Conflict(new { message = "A step with this order already exists in the workflow." });
        }

        var step = await _stepService.CreateStepAsync(workflowId, stepDto);
        return CreatedAtAction(nameof(GetStepById), new { workflowId, id = step.Id }, ToResponseDto(step));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StepResponseDto>>> GetAllSteps(int workflowId)
    {
        if (!await _stepService.WorkflowExistsAsync(workflowId))
        {
            return NotFound();
        }

        var steps = await _stepService.GetStepsByWorkflowIdAsync(workflowId);
        return Ok(steps.Select(ToResponseDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StepResponseDto>> GetStepById(int workflowId, int id)
    {
        var step = await _stepService.GetStepByIdAsync(workflowId, id);
        return step is null ? NotFound() : Ok(ToResponseDto(step));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<StepResponseDto>> UpdateStep(int workflowId, int id, UpdateStepDto stepDto)
    {
        if (!await _stepService.WorkflowExistsAsync(workflowId))
        {
            return NotFound();
        }

        if (!TryValidate(stepDto.Order, stepDto.ActionType, stepDto.ConfigJson, out var validationResult))
        {
            return validationResult;
        }

        if (await _stepService.IsOrderInUseAsync(workflowId, stepDto.Order, id))
        {
            return Conflict(new { message = "A step with this order already exists in the workflow." });
        }

        var step = await _stepService.UpdateStepAsync(workflowId, id, stepDto);
        return step is null ? NotFound() : Ok(ToResponseDto(step));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteStep(int workflowId, int id)
    {
        if (!await _stepService.WorkflowExistsAsync(workflowId))
        {
            return NotFound();
        }

        return await _stepService.DeleteStepAsync(workflowId, id) ? NoContent() : NotFound();
    }

    private bool TryValidate(int order, ActionType actionType, string configJson, out ActionResult validationResult)
    {
        if (order < 0)
        {
            ModelState.AddModelError("order", "Order must be zero or greater.");
        }

        if (!ConfigurationJsonValidator.TryValidateStep(actionType, configJson, out var configError))
        {
            ModelState.AddModelError("configJson", configError);
        }

        validationResult = ValidationProblem(ModelState);
        return ModelState.IsValid;
    }

    private static StepResponseDto ToResponseDto(Step step)
    {
        return new StepResponseDto
        {
            Id = step.Id,
            WorkflowId = step.WorkflowId,
            Order = step.Order,
            ActionType = step.ActionType,
            ConfigJson = step.ConfigJson
        };
    }
}
