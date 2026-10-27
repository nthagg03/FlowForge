using FlowForge.Models.Enums;

namespace FlowForge.Services.ExecutionEngine;

public interface IActionHandler
{
    ActionType ActionType { get; }
    Task<ActionExecutionResult> ExecuteAsync(string configJson, CancellationToken cancellationToken = default);
}
