using FlowForge.Data;
using FlowForge.Models.Entities;
using FlowForge.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowForge.Repositories;

public class ExecutionRepository : IExecutionRepository
{
    private readonly AppDbContext _dbContext;

    public ExecutionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<WorkflowExecution> CreateExecutionAsync(WorkflowExecution execution)
    {
        _dbContext.WorkflowExecutions.Add(execution);
        await _dbContext.SaveChangesAsync();
        return execution;
    }

    public async Task<StepExecutionLog> CreateStepLogAsync(StepExecutionLog log)
    {
        _dbContext.StepExecutionLogs.Add(log);
        await _dbContext.SaveChangesAsync();
        return log;
    }

    public Task SaveChangesAsync()
    {
        return _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Returns a single execution with its step logs ordered by StartedAt ascending,
    /// each log includes the associated Step for Order and ActionType.
    /// </summary>
    public Task<WorkflowExecution?> GetExecutionByIdAsync(int id)
    {
        return _dbContext.WorkflowExecutions
            .Include(e => e.StepExecutionLogs.OrderBy(l => l.StartedAt))
                .ThenInclude(l => l.Step)
            .SingleOrDefaultAsync(e => e.Id == id);
    }

    /// <summary>
    /// Returns paginated executions for a workflow, newest first.
    /// Uses the composite index (WorkflowId, StartedAt) defined in AppDbContext.
    /// </summary>
    public Task<List<WorkflowExecution>> GetExecutionsByWorkflowIdAsync(int workflowId, int page, int pageSize)
    {
        return _dbContext.WorkflowExecutions
            .Where(e => e.WorkflowId == workflowId)
            .OrderByDescending(e => e.StartedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}
