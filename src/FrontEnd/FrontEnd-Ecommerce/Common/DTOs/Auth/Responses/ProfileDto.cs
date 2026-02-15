namespace FrontEnd_Ecommerce.DTOs.Auth.Responses;

public record ProfileDto(Guid Id, string FirstName,string LastName, string Email, List<string> Roles);