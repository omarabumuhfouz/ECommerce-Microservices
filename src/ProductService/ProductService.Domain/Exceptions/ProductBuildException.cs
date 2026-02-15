using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Exception thrown when ProductBuilder fails to build a valid Product.
/// Contains detailed validation errors.
/// </summary>
public class ProductBuildException : Exception
{
    /// <summary>
    /// Gets the list of validation errors that caused the build to fail.
    /// </summary>
    public IReadOnlyList<string> ValidationErrors { get; }

    /// <summary>
    /// Initializes a new instance of ProductBuildException with validation errors.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="validationErrors">The collection of validation errors.</param>
    public ProductBuildException(string message, IEnumerable<string> validationErrors) 
        : base(message)
    {
        ValidationErrors = validationErrors?.ToList().AsReadOnly() 
            ?? new List<string>().AsReadOnly();
    }

    /// <summary>
    /// Initializes a new instance of ProductBuildException with a single error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ProductBuildException(string message) 
        : base(message)
    {
        ValidationErrors = new List<string> { message }.AsReadOnly();
    }

    /// <summary>
    /// Initializes a new instance of ProductBuildException with an inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that caused this exception.</param>
    public ProductBuildException(string message, Exception innerException) 
        : base(message, innerException)
    {
        ValidationErrors = new List<string> { message }.AsReadOnly();
    }

    /// <summary>
    /// Gets the formatted error message including all validation errors.
    /// </summary>
    public override string Message
    {
        get
        {
            if (!ValidationErrors.Any())
                return base.Message;

            var baseMessage = base.Message;
            var errors = string.Join(Environment.NewLine + "  - ", ValidationErrors);
            return $"{baseMessage}{Environment.NewLine}  - {errors}";
        }
    }

    /// <summary>
    /// Returns a string representation of the exception with all validation errors.
    /// </summary>
    public override string ToString()
    {
        return $"{GetType().Name}: {Message}{Environment.NewLine}{StackTrace}";
    }
}