namespace AuthService.Application.Features.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid UserId) : ICommand<Unit>;