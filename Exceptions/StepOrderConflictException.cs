namespace FlowForge.Exceptions;

public class StepOrderConflictException : ConflictException
{
    public StepOrderConflictException() : base("A step with this order already exists in the workflow.")
    {
    }
}
