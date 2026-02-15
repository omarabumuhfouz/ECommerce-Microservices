using AuthService.Application.Extensions;

namespace AuthService.Application.Features.RefreshTokens.Refresh;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.RefreshToken.TokenRequired)
            .WithMessage("Refresh token is required.");

        RuleFor(x => x.ClientId)
            .ValidateClientId();
    }
}