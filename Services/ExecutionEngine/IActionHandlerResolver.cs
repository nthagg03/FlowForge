using System.Diagnostics.CodeAnalysis;
using FlowForge.Models.Enums;

namespace FlowForge.Services.ExecutionEngine;

public interface IActionHandlerResolver
{
    IActionHandler GetHandler(ActionType actionType);
    bool TryGetHandler(ActionType actionType, [NotNullWhen(true)] out IActionHandler? handler);
}
