using FrontEnd_Ecommerce.DTOs.Auth.Requests;

namespace FrontEnd_Ecommerce.ViewModels;

public class AuthPageViewModel
{
    public bool ShowLogin { get; set; } = true;

    public LoginRequestDto Login { get; set; } = new("", "", "");
    public RegisterRequestDto Register { get; set; } = new("", "", "", "", "", "");
}
