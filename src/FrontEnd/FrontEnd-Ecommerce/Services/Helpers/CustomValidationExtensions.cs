using FluentValidation;

public static class CustomValidationExtensions
{
    // Email validation
    public static IRuleBuilderOptions<T, string> ValidateEmail<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }

    // Password validation
    public static IRuleBuilderOptions<T, string> ValidatePassword<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches(@"\d").WithMessage("Password must contain at least one number");
    }

    // ClientId validation
    
    public static IRuleBuilderOptions<T, string> ValidateClientId<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("ClientId is required");
    }
    /// <summary>
    /// Validates that a string property is not empty.
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateRequired<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        string errorMessage = "This field is required")
    {
        return ruleBuilder
            .NotEmpty().WithMessage(errorMessage);
    }

}
