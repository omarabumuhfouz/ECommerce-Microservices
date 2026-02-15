  namespace FrontEnd_Ecommerce.Exceptions;
  /// <summary>
    /// Represents a bad request error (HTTP 400)
    /// </summary>
    public class BadRequestException : Exception
    {
        public BadRequestException() : base() { }

        public BadRequestException(string message) : base(message) { }

        public BadRequestException(string message, Exception innerException) 
            : base(message, innerException) { }
    }