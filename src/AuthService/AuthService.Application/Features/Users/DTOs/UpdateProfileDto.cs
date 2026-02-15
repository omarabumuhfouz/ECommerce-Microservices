namespace AuthService.Application.Features.Users.DTOs;

public record UpdateProfileDto(string FirstName, string LastName, string Email, string Password);
