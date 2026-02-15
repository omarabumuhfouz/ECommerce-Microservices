using CancellationService.Domain.Constants;

namespace CancellationService.Application.Common.Extensions;

public static class CancellationValidationExtensions
{
    /// <summary>
    /// Reusable validation for Cancellation ID with Error Code
    /// </summary>
    public static IRuleBuilderOptions<T, Guid> ValidateCancellationId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Cancellation.IdRequired) // <--- The Error Code
            .WithMessage("Cancellation ID is required and cannot be empty.");
    }

    /// <summary>
    /// Reusable validation for Reason with Error Code
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateReason<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Reason is required.")
            .MaximumLength(CancellationConstants.MaxReasonLength)
            .WithMessage($"Reason cannot exceed {CancellationConstants.MaxReasonLength} characters.");

    }

    /// <summary>
    /// Reusable validation for Admin/User ID with Error Code
    /// </summary>
    public static IRuleBuilderOptions<T, Guid> ValidateAdminId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Cancellation.AdminIdRequired) // <--- The Error Code
            .WithMessage("Admin ID is required and cannot be empty.");
    }

    /// <summary>
    /// Reusable validation for Admin/User ID with Error Code
    /// </summary>
    public static IRuleBuilderOptions<T, Guid> ValidateOrderId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Order.IdRequired) // <--- The Error Code
            .WithMessage("Order ID is required and cannot be empty.");
    }

    public static IRuleBuilderOptions<T, string> ValidateRemarks<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Remarks are required for a rejection.")
            .MaximumLength(CancellationConstants.MaxRemarksLength)
            .WithMessage($"Remarks cannot exceed {CancellationConstants.MaxRemarksLength} characters.");

    }
}