using FluentValidation;
using FrontEnd_Ecommerce.DTOs.Auth.Requests;

namespace FrontEnd_Ecommerce.Validators.Auth;

public class LogoutRequestDtoValidator : AbstractValidator<LogoutRequestDto>
{
    public LogoutRequestDtoValidator()
    {
        RuleFor(x => x.RefreshToken)
            .ValidateRequired("Refresh token is required");

        RuleFor(x => x.ClientId)
            .ValidateRequired("ClientId is required");
    }
}
