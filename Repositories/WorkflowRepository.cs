using Microsoft.EntityFrameworkCore;
using FlowForge.Repositories.Interfaces;
using FlowForge.Data;             
using FlowForge.Models.Entities;

namespace FlowForge.Repositories;

public class WorkflowRepository : IWorkflowRepository
{
    private readonly AppDbContext _dbcontext;

    public WorkflowRepository(AppDbContext dbContext)
    {
        _dbcontext = dbContext;
    }

    public async Task<Workflow?> GetWorkflowByIdAsync(int id)
    {
        return await _dbcontext.Workflows.FindAsync(id);
    }

    public async Task<List<Workflow>> GetAllWorkflowsAsync()
    {
        return await _dbcontext.Workflows.ToListAsync();
    }

    public async Task<Workflow> CreateWorkflowAsync(Workflow workflow)
    {
        _dbcontext.Workflows.Add(workflow);
        await _dbcontext.SaveChangesAsync();
        return workflow;
    }

    public async Task<Workflow?> UpdateWorkflowAsync(Workflow workflow)
    {
        var existingWorkflow = await _dbcontext.Workflows.FindAsync(workflow.Id);
        if (existingWorkflow == null)
        {
            return null;
        }

        existingWorkflow.Name = workflow.Name;
        existingWorkflow.Status = workflow.Status;

        _dbcontext.Workflows.Update(existingWorkflow);
        await _dbcontext.SaveChangesAsync();

        return existingWorkflow;
    }

    public async Task<bool> DeleteWorkflowAsync(int id)
    {
        var workflow = await _dbcontext.Workflows.FindAsync(id);
        if (workflow == null)
        {
            return false;
        }

        _dbcontext.Workflows.Remove(workflow);
        await _dbcontext.SaveChangesAsync();
        return true;
    }
}