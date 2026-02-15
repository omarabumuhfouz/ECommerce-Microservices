namespace AuthService.Application.Features.Users.Commands.ChangeRole;

public record ChangeRoleCommand(Guid UserId, List<string> Roles) : ICommand<Unit>;
