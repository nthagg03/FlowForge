using System.Text.Json;
using FlowForge.Models.Enums;

namespace FlowForge.Services;

public static class ConfigurationJsonValidator
{
    public static bool TryValidateStep(ActionType actionType, string configJson, out string errorMessage)
    {
        if (!TryReadObject(configJson, out var config, out errorMessage))
        {
            return false;
        }

        if (!Enum.IsDefined(actionType))
        {
            errorMessage = "ActionType is not supported.";
            return false;
        }

        switch (actionType)
        {
            case ActionType.WriteLog:
                return RequireString(config, "message", out errorMessage);

            case ActionType.CallApi:
                if (!RequireString(config, "url", out var url, out errorMessage) ||
                    !Uri.TryCreate(url, UriKind.Absolute, out _))
                {
                    errorMessage = "ConfigJson must contain a valid absolute url for CallApi.";
                    return false;
                }

                if (!RequireString(config, "method", out var method, out errorMessage) ||
                    !IsHttpMethod(method))
                {
                    errorMessage = "ConfigJson must contain a supported HTTP method for CallApi.";
                    return false;
                }

                if (config.TryGetProperty("headers", out var headers) &&
                    headers.ValueKind is not JsonValueKind.Object and not JsonValueKind.Null)
                {
                    errorMessage = "headers must be a JSON object when provided.";
                    return false;
                }

                errorMessage = string.Empty;
                return true;

            case ActionType.SendEmail:
                return RequireString(config, "to", out errorMessage) &&
                    RequireString(config, "subject", out errorMessage) &&
                    RequireString(config, "body", out errorMessage);

            default:
                errorMessage = "ActionType is not supported.";
                return false;
        }
    }

    public static bool TryValidateTrigger(TriggerType triggerType, string configJson, out string errorMessage)
    {
        if (!TryReadObject(configJson, out var config, out errorMessage))
        {
            return false;
        }

        if (!Enum.IsDefined(triggerType))
        {
            errorMessage = "TriggerType is not supported.";
            return false;
        }

        switch (triggerType)
        {
            case TriggerType.Schedule:
                if (!RequireString(config, "cron", out var cron, out errorMessage) || !IsCronExpression(cron))
                {
                    errorMessage = "ConfigJson must contain a five-part cron expression for Schedule.";
                    return false;
                }

                return true;

            case TriggerType.Webhook:
                return RequireString(config, "secret", out errorMessage);

            case TriggerType.Manual:
                errorMessage = string.Empty;
                return true;

            default:
                errorMessage = "TriggerType is not supported.";
                return false;
        }
    }

    private static bool TryReadObject(string configJson, out JsonElement config, out string errorMessage)
    {
        try
        {
            using var document = JsonDocument.Parse(configJson);
            config = document.RootElement.Clone();
        }
        catch (JsonException)
        {
            config = default;
            errorMessage = "ConfigJson must be valid JSON.";
            return false;
        }

        if (config.ValueKind != JsonValueKind.Object)
        {
            errorMessage = "ConfigJson must be a JSON object.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static bool RequireString(JsonElement config, string propertyName, out string value, out string errorMessage)
    {
        if (!config.TryGetProperty(propertyName, out var property) ||
            property.ValueKind != JsonValueKind.String ||
            string.IsNullOrWhiteSpace(property.GetString()))
        {
            value = string.Empty;
            errorMessage = $"ConfigJson must contain a non-empty {propertyName}.";
            return false;
        }

        value = property.GetString()!;
        errorMessage = string.Empty;
        return true;
    }

    private static bool RequireString(JsonElement config, string propertyName, out string errorMessage)
    {
        return RequireString(config, propertyName, out _, out errorMessage);
    }

    private static bool IsHttpMethod(string method)
    {
        return method.ToUpperInvariant() is "GET" or "POST" or "PUT" or "PATCH" or "DELETE" or "HEAD" or "OPTIONS";
    }

    private static bool IsCronExpression(string cron)
    {
        return cron.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length == 5;
    }
}
