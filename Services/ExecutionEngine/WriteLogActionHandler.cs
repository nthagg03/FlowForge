using System.Text.Json;
using FlowForge.Models.Enums;
using Microsoft.Extensions.Logging;

namespace FlowForge.Services.ExecutionEngine;

public class WriteLogActionHandler : IActionHandler
{
    private readonly ILogger<WriteLogActionHandler> _logger;

    public WriteLogActionHandler(ILogger<WriteLogActionHandler> logger)
    {
        _logger = logger;
    }

    public ActionType ActionType => ActionType.WriteLog;

    public Task<ActionExecutionResult> ExecuteAsync(string configJson, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(configJson))
        {
            return Task.FromResult(ActionExecutionResult.Failed("ConfigJson cannot be empty.", isRetryable: false));
        }

        WriteLogConfig? config;
        try
        {
            config = JsonSerializer.Deserialize<WriteLogConfig>(configJson, ActionJsonSerializerOptions.Default);
        }
        catch (JsonException ex)
        {
            return Task.FromResult(ActionExecutionResult.Failed($"Invalid configuration JSON: {ex.Message}", isRetryable: false));
        }

        if (config is null || string.IsNullOrWhiteSpace(config.Message))
        {
            return Task.FromResult(ActionExecutionResult.Failed("Message is required.", isRetryable: false));
        }

        _logger.LogInformation("{Message}", config.Message);

        return Task.FromResult(ActionExecutionResult.Succeeded(config.Message));
    }
}
