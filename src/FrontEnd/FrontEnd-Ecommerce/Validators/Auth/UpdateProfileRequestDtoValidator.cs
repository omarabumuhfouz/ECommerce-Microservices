using FrontEnd_Ecommerce.DTOs.Auth.Requests;

namespace FluentValidation.Validators.Auth;

public class UpdateProfileRequestDtoValidator : AbstractValidator<EditProfileRequestDto>
{
    public UpdateProfileRequestDtoValidator()
    {
        // First and last name validations
        RuleFor(x => x.FirstName)
            .ValidateRequired("First name is required")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters");

        RuleFor(x => x.LastName)
            .ValidateRequired("Last name is required")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters");

        // Email validation
        RuleFor(x => x.Email)
            .ValidateEmail();
    }
}
