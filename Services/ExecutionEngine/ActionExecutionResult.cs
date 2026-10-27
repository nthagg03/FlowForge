namespace FlowForge.Services.ExecutionEngine;

public class ActionExecutionResult
{
    public bool Success { get; }
    public string? Output { get; }
    public string? ErrorMessage { get; }
    public bool IsRetryable { get; }

    private ActionExecutionResult(bool success, string? output, string? errorMessage, bool isRetryable)
    {
        Success = success;
        Output = output;
        ErrorMessage = errorMessage;
        IsRetryable = isRetryable;
    }

    public static ActionExecutionResult Succeeded(string? output = null)
    {
        return new ActionExecutionResult(true, output, null, false);
    }

    public static ActionExecutionResult Failed(string errorMessage, bool isRetryable = false, string? output = null)
    {
        return new ActionExecutionResult(false, output, errorMessage, isRetryable);
    }
}
