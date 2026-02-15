using FluentValidation;
using FrontEnd_Ecommerce.DTOs.Auth.Requests;

namespace FrontEnd_Ecommerce.Validators.Auth;

public class ChangePasswordRequestDtoValidator : AbstractValidator<ChangePasswordRequestDto>
{
    public ChangePasswordRequestDtoValidator()
    {
        // Current password validation (basic required)
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Current password is required");

        // New password validation (reuse extension)
        RuleFor(x => x.NewPassword)
            .ValidatePassword();

        // Confirm new password must match NewPassword
        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty().WithMessage("Confirm new password is required")
            .Equal(x => x.NewPassword).WithMessage("Passwords do not match")
            .ValidatePassword(); // optional: enforce same password rules here too
    }
}
