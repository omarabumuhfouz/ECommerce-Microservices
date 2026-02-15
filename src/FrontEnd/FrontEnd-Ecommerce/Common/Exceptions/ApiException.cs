namespace FrontEnd_Ecommerce.Exceptions;

public class ApiException : Exception
{
    public int StatusCode { get; }
    public string? ResponseContent { get; }

    public ApiException(int statusCode, string? content, string message) 
        : base(message)
    {
        StatusCode = statusCode;
        ResponseContent = content;
    }
}