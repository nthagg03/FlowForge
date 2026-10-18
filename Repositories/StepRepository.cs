using FlowForge.Data;
using FlowForge.Models.Entities;
using FlowForge.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowForge.Repositories;

public class StepRepository : IStepRepository
{
    private readonly AppDbContext _dbContext;

    public StepRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Step>> GetStepsByWorkflowIdAsync(int workflowId)
    {
        return _dbContext.Steps
            .Where(step => step.WorkflowId == workflowId)
            .OrderBy(step => step.Order)
            .ToListAsync();
    }

    public Task<Step?> GetStepByIdAsync(int workflowId, int id)
    {
        return _dbContext.Steps
            .SingleOrDefaultAsync(step => step.WorkflowId == workflowId && step.Id == id);
    }

    public async Task<Step> CreateStepAsync(Step step)
    {
        _dbContext.Steps.Add(step);
        await _dbContext.SaveChangesAsync();
        return step;
    }

    public async Task<Step> UpdateStepAsync(Step step)
    {
        _dbContext.Steps.Update(step);
        await _dbContext.SaveChangesAsync();
        return step;
    }

    public async Task<bool> DeleteStepAsync(Step step)
    {
        _dbContext.Steps.Remove(step);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public Task<bool> IsOrderInUseAsync(int workflowId, int order, int? excludedStepId = null)
    {
        return _dbContext.Steps.AnyAsync(step =>
            step.WorkflowId == workflowId &&
            step.Order == order &&
            (!excludedStepId.HasValue || step.Id != excludedStepId.Value));
    }
}
