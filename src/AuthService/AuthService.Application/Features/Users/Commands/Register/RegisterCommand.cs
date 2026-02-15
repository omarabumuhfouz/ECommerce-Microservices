namespace AuthService.Features.Users.Commands.Register;
public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Password,
    string ClientId

) : ICommand<RegisterResponseDto>;
