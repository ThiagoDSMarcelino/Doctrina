namespace Doctrina.Exceptions.DecisionTreeExceptions;

public class InvalidParameterSizeException : System.Exception
{
    public InvalidParameterSizeException() { }
    public InvalidParameterSizeException(string message) : base(message) { }
    public InvalidParameterSizeException(string message, System.Exception inner) : base(message, inner) { }
    protected InvalidParameterSizeException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

    public override string Message =>
        "The array of inputs is not the same size";
}