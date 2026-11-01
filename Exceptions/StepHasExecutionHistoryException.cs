namespace FlowForge.Exceptions;

public class StepHasExecutionHistoryException : ConflictException
{
    public StepHasExecutionHistoryException() : base("Step has execution history and cannot be deleted.")
    {
    }
}
