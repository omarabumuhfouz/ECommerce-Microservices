using FluentValidation;
using FrontEnd_Ecommerce.DTOs.Auth.Requests;

namespace FrontEnd_Ecommerce.Validators.Auth;
public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Email).ValidateEmail();
        RuleFor(x => x.Password)
            .ValidateRequired("Password Required.")
            .MinimumLength(6).WithMessage("Password must be at lest 6 character.");
        RuleFor(x => x.ClientId).ValidateClientId();
    }
}
