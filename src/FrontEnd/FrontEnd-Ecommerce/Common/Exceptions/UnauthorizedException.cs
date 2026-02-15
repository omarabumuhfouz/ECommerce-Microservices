     namespace FrontEnd_Ecommerce.Exceptions;
     /// <summary>
    /// Represents an unauthorized access error (HTTP 401)
    /// </summary>
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base() { }

        public UnauthorizedException(string message) : base(message) { }

        public UnauthorizedException(string message, Exception innerException) 
            : base(message, innerException) { }
    }