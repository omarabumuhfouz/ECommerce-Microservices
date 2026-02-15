namespace AuthService.Application.Features.Users.DTOs;

public record ProfileDto(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    List<string> Roles);
