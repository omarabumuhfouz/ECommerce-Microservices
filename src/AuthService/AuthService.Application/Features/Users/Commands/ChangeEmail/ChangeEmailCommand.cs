namespace AuthService.Application.Features.Users.Commands.ChangeEmail;

public record ChangeEmailCommand(Guid UserId, string NewEmail) : ICommand<Unit>;
