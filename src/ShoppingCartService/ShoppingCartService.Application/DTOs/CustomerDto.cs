namespace ShoppingCartService.Application.DTOs;

public record CustomerDto(
    Guid Id,
    Guid UserId,
    string FirstName,
    string LastName,
    string PhoneNumber);
