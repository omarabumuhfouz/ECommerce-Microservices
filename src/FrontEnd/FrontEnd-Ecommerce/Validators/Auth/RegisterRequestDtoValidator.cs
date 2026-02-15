using FluentValidation;
using FrontEnd_Ecommerce.DTOs.Auth.Requests;

namespace FrontEnd_Ecommerce.Validators.Auth;

public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestDtoValidator()
    {
        // First and last name required
        RuleFor(x => x.FirstName)
            .ValidateRequired("First name is required")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters");

        RuleFor(x => x.LastName)
            .ValidateRequired("Last name is required")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters");

        // Email validation
        RuleFor(x => x.Email)
            .ValidateEmail();

        // Password validation
        RuleFor(x => x.Password)
            .ValidatePassword();

        // Confirm password validation
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm new password is required")
            .Equal(x => x.Password).WithMessage("Passwords do not match")
            .ValidatePassword(); // optional: enforce same password rules here too


        // ClientId required
        RuleFor(x => x.ClientId)
            .ValidateRequired("ClientId is required");
    }
}
