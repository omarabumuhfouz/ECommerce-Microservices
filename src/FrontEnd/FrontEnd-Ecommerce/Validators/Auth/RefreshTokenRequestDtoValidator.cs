using FluentValidation;
using FrontEnd_Ecommerce.DTOs.Auth.Requests;

namespace FrontEnd_Ecommerce.Validators.Auth;

public class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshTokenRequestDto>
{
    public RefreshTokenRequestDtoValidator()
    {
        RuleFor(x => x.RefreshToken)
            .ValidateRequired("Refresh token is required");

        RuleFor(x => x.ClientId)
            .ValidateRequired("ClientId is required");
    }
}
