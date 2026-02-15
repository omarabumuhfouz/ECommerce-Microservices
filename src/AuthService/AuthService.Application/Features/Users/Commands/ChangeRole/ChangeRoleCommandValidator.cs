namespace AuthService.Application.Features.Users.Commands.ChangeRole;

public class ChangeRoleCommandValidator : AbstractValidator<ChangeRoleCommand>
{
    public ChangeRoleCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Roles).NotEmpty();
        RuleForEach(x => x.Roles).Must(role => Enum.TryParse<UserRole>(role, true, out _))
            .WithMessage("Invalid role specified.");
    }
}
