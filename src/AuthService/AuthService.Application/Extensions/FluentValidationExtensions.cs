namespace AuthService.Application.Extensions;

public static class FluentValidationExtensions
{
    /// <summary>
    /// Custom rule to validate email format using shared logic and standard error codes.
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.User.EmailRequired)
            .WithMessage("Email address is required.")
        
            // Reuses your SharedKernel extension
            .Must(email => email.IsValidEmail()) 
            .WithErrorCode(ErrorCodes.User.EmailInvalid)
            .WithMessage("The provided email format is invalid.");
    }

    /// <summary>
    /// Custom rule to validate URL format using shared logic and standard error codes.
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateUrl<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Client.UrlRequired) // Assuming you have this or ClientIdRequired context
        
            // Reuses your SharedKernel extension
            .Must(url => url.IsValidUrl())
            .WithErrorCode(ErrorCodes.Client.UrlInvalid)
            .WithMessage("The provided URL format is invalid.");
    }

    /// <summary>
    /// Helper for Password required check to keep code clean
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidatePassword<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.User.PasswordRequired)
            .WithMessage("Password is required.");
    }

/// <summary>
    /// Custom rule to validate ClientId (NotEmpty + Standard Error Code).
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateClientId<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Client.ClientIdRequired)
            .WithMessage("Client ID is required.");
    }
}