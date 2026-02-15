using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FrontEnd_Ecommerce.DTOs.Auth.Requests;
    
public record LoginRequest(
    string Email,
    string Password,
    string ClientId = "myapp"
)
{
    public static LoginRequest Empty() =>  new("","");
}