namespace API.Exceptions;

public class ApiException
{
    private int StatusCode;
    private string Message;
    private string Details;


    public ApiException(int statusCode, string message, string details)
    {
        StatusCode = statusCode;
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Details = details ?? throw new ArgumentNullException(nameof(details));
    }
}