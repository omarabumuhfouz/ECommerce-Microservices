using AuthService.Application.Extensions;
using AuthService.Features.Users.Commands.Logout;

namespace AuthService.Application.Features.Users.Commands.Logout;

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.User.IdRequired)
            .WithMessage("User ID is required.");

        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.RefreshToken.TokenRequired)
            .WithMessage("Refresh token is required.");

        RuleFor(x => x.ClientId)
            .ValidateClientId();
    }
}