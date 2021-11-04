namespace Lithium.Api;

public class HttpResponseException : Exception
{
    public HttpResponseException(int status, object value)
    {
        Status = status;
        Value = value;
    }

    public HttpResponseException(int status, object value, string? message, Exception? innerException) : base(message, innerException)
    {
        Status = status;
        Value = value;
    }

    public int Status { get; init; }

    public object Value { get; init; }
}