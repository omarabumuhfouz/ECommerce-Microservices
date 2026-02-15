namespace FrontEnd_Ecommerce.DTOs.Auth.Requests;

public record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword
)
{
    public static ChangePasswordRequest Empty() => new("", "", "");
}