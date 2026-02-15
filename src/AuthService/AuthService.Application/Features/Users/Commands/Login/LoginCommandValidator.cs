using AuthService.Application.Extensions;
using AuthService.Application.Features.Users.Commands.Login;

namespace AuthService.Users.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).ValidateEmail(); 

        RuleFor(x => x.Password).ValidatePassword();

        RuleFor(x => x.ClientId).ValidateClientId();
    }
}
