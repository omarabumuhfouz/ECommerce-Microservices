namespace FrontEnd_Ecommerce.Exceptions;

/// <summary>
/// Thrown when an expected authentication token is missing.
/// </summary>
public class MissingTokenException : Exception
{
    public MissingTokenException() : base(){}
    
    public MissingTokenException(string message) 
        : base(message) { }
}
