namespace FrontEnd_Ecommerce.Bases;

public static class Endpoints
{
    public static class Auth
    {
        private const string baseUrl = "/api/auth";
        public const string Register = $"{baseUrl}/register";
        public const string Login = $"{baseUrl}/login";
        public const string Logout = $"{baseUrl}/logout";
        public const string ChangePassword = $"{baseUrl}/change-password";
        public const string RefreshToken = $"{baseUrl}/refresh-token";
        public const string UpdateProfile = $"{baseUrl}/update-profile";
    }
    
}