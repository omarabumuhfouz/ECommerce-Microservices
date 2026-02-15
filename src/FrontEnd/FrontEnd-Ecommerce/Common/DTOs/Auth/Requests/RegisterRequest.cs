namespace FrontEnd_Ecommerce.DTOs.Auth.Requests;

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword,
    string ClientId
)
{
    public static RegisterRequest Empty() => new RegisterRequest("", "", "", "", "", "");
}