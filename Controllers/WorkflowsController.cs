using FlowForge.Models.DTOs;
using FlowForge.Models.Entities;
using FlowForge.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowForge.Controllers;

[ApiController]
[Route("api/workflows")]
public class WorkflowsController : ControllerBase
{
    private readonly IWorkflowService _workflowService;

    public WorkflowsController(IWorkflowService workflowService)
    {
        _workflowService = workflowService;
    }

    [HttpPost]
    public async Task<ActionResult<WorkflowResponseDto>> CreateWorkflow(
        CreateWorkflowDto workflowDto)
    {
        var workflow = await _workflowService.CreateWorkflowAsync(workflowDto);
        var response = ToResponseDto(workflow);

        return CreatedAtAction(nameof(GetWorkflowById), new { id = workflow.Id }, response);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkflowResponseDto>>> GetAllWorkflows()
    {
        var workflows = await _workflowService.GetAllWorkflowsAsync();
        return Ok(workflows.Select(ToResponseDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkflowResponseDto>> GetWorkflowById(int id)
    {
        var workflow = await _workflowService.GetWorkflowByIdAsync(id);
        return workflow is null ? NotFound() : Ok(ToResponseDto(workflow));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<WorkflowResponseDto>> UpdateWorkflow(
        int id,
        UpdateWorkflowDto workflowDto)
    {
        var workflow = await _workflowService.UpdateWorkflowAsync(id, workflowDto);
        return workflow is null ? NotFound() : Ok(ToResponseDto(workflow));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWorkflow(int id)
    {
        var deleted = await _workflowService.DeleteWorkflowAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    private static WorkflowResponseDto ToResponseDto(Workflow workflow)
    {
        return new WorkflowResponseDto
        {
            Id = workflow.Id,
            Name = workflow.Name,
            Status = workflow.Status,
            CreatedBy = workflow.CreatedBy,
            CreatedAt = workflow.CreatedAt
        };
    }
}
