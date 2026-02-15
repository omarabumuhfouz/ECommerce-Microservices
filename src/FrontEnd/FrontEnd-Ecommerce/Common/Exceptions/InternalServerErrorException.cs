namespace FrontEnd_Ecommerce.Exceptions;
 /// <summary>
    /// Represents an internal server error (HTTP 500)
    /// </summary>
    public class InternalServerErrorException : Exception
    {
        public InternalServerErrorException(): base() { }

        public InternalServerErrorException(string message) : base(message) { }

        public InternalServerErrorException(string message, Exception innerException) 
            : base(message, innerException) { }
    }