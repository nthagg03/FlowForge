using System.Text.Json;
using FlowForge.Models.Enums;
using Microsoft.Extensions.Logging;

namespace FlowForge.Services.ExecutionEngine;

public class SendEmailActionHandler : IActionHandler
{
    private readonly ILogger<SendEmailActionHandler> _logger;

    public SendEmailActionHandler(ILogger<SendEmailActionHandler> logger)
    {
        _logger = logger;
    }

    public ActionType ActionType => ActionType.SendEmail;

    public Task<ActionExecutionResult> ExecuteAsync(string configJson, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(configJson))
        {
            return Task.FromResult(ActionExecutionResult.Failed("ConfigJson cannot be empty.", isRetryable: false));
        }

        SendEmailConfig? config;
        try
        {
            config = JsonSerializer.Deserialize<SendEmailConfig>(configJson, ActionJsonSerializerOptions.Default);
        }
        catch (JsonException ex)
        {
            return Task.FromResult(ActionExecutionResult.Failed($"Invalid configuration JSON: {ex.Message}", isRetryable: false));
        }

        if (config is null)
        {
            return Task.FromResult(ActionExecutionResult.Failed("Configuration is missing.", isRetryable: false));
        }

        if (string.IsNullOrWhiteSpace(config.To))
        {
            return Task.FromResult(ActionExecutionResult.Failed("To is required.", isRetryable: false));
        }

        if (string.IsNullOrWhiteSpace(config.Subject))
        {
            return Task.FromResult(ActionExecutionResult.Failed("Subject is required.", isRetryable: false));
        }

        if (string.IsNullOrWhiteSpace(config.Body))
        {
            return Task.FromResult(ActionExecutionResult.Failed("Body is required.", isRetryable: false));
        }

        _logger.LogInformation("would send to {To} with subject '{Subject}'", config.To, config.Subject);

        return Task.FromResult(ActionExecutionResult.Succeeded($"Email would be sent to {config.To}"));
    }
}
