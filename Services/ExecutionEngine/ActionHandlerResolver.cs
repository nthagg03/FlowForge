using System.Diagnostics.CodeAnalysis;
using FlowForge.Models.Enums;

namespace FlowForge.Services.ExecutionEngine;

public class ActionHandlerResolver : IActionHandlerResolver
{
    private readonly Dictionary<ActionType, IActionHandler> _handlers = new();

    public ActionHandlerResolver(IEnumerable<IActionHandler> handlers)
    {
        foreach (var handler in handlers)
        {
            if (!_handlers.TryAdd(handler.ActionType, handler))
            {
                throw new InvalidOperationException($"Duplicate action handler registered for ActionType '{handler.ActionType}'.");
            }
        }
    }

    public IActionHandler GetHandler(ActionType actionType)
    {
        if (_handlers.TryGetValue(actionType, out var handler))
        {
            return handler;
        }

        throw new NotSupportedException($"No action handler registered for ActionType '{actionType}'.");
    }

    public bool TryGetHandler(ActionType actionType, [NotNullWhen(true)] out IActionHandler? handler)
    {
        return _handlers.TryGetValue(actionType, out handler);
    }
}
