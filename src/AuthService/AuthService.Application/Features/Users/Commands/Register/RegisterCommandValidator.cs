using AuthService.Application.Extensions;

namespace AuthService.Features.Users.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.User.FirstNameRequired)
            .WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.User.LastNameRequired)
            .WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

        RuleFor(x => x.Email)
            .ValidateEmail();

        RuleFor(x => x.Password)
            .ValidatePassword() // Checks NotEmpty
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

        RuleFor(x => x.ClientId)
            .ValidateClientId();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.User.PhoneNumberRequired)
            .WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$") 
            .WithErrorCode(ErrorCodes.User.PhoneNumberInvalid)
            .WithMessage("Invalid phone number format.");
    }
}