namespace FlowForge.Exceptions;

public class StepOrderConflictException : Exception
{
    public StepOrderConflictException() : base("A step with this order already exists in the workflow.")
    {
    }
}
