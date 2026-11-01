using FlowForge.Models.Entities;

namespace FlowForge.Repositories.Interfaces;

public interface IExecutionRepository
{
    Task<WorkflowExecution> CreateExecutionAsync(WorkflowExecution execution);
    Task<StepExecutionLog> CreateStepLogAsync(StepExecutionLog log);
    Task SaveChangesAsync();
    Task<WorkflowExecution?> GetExecutionByIdAsync(int id);
    Task<List<WorkflowExecution>> GetExecutionsByWorkflowIdAsync(int workflowId, int page, int pageSize);
}
