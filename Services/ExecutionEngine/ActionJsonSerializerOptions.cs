using System.Text.Json;

namespace FlowForge.Services.ExecutionEngine;

public static class ActionJsonSerializerOptions
{
    public static readonly JsonSerializerOptions Default = new()
    {
        PropertyNameCaseInsensitive = true
    };
}
