using FlowForge.Data;
using FlowForge.Models.Entities;
using FlowForge.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowForge.Repositories;

public class TriggerRepository : ITriggerRepository
{
    private readonly AppDbContext _dbContext;

    public TriggerRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Trigger>> GetTriggersByWorkflowIdAsync(int workflowId)
    {
        return _dbContext.Triggers
            .Where(trigger => trigger.WorkflowId == workflowId)
            .ToListAsync();
    }

    public Task<Trigger?> GetTriggerByIdAsync(int workflowId, int id)
    {
        return _dbContext.Triggers
            .SingleOrDefaultAsync(trigger => trigger.WorkflowId == workflowId && trigger.Id == id);
    }

    public async Task<Trigger> CreateTriggerAsync(Trigger trigger)
    {
        _dbContext.Triggers.Add(trigger);
        await _dbContext.SaveChangesAsync();
        return trigger;
    }

    public async Task<Trigger> UpdateTriggerAsync(Trigger trigger)
    {
        _dbContext.Triggers.Update(trigger);
        await _dbContext.SaveChangesAsync();
        return trigger;
    }

    public async Task<bool> DeleteTriggerAsync(Trigger trigger)
    {
        _dbContext.Triggers.Remove(trigger);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
