namespace AuthService.Domain.Errors;

public static class ErrorCodes
{
    public static class User
    {
        public const string EmailAlreadyExists = "User.EmailAlreadyExists";
        public const string RolesRequired = "User.RolesRequired";
        public const string RoleInvalid = "User.RoleInvalid";
        public const string NotFound = "User.NotFound";
        public const string EmailNotUnique = "User.EmailNotUnique";
        public const string InvalidCredentials = "User.InvalidCredentials"; // ✅ We will use this for Login
        public const string NotActive = "User.NotActive";
        public const string AlreadyVerified = "User.AlreadyVerified";

        // Validation codes...
        public const string IdRequired = "User.IdRequired";
        public const string CurrentPasswordRequired = "User.CurrentPasswordRequired";
        public const string NewPasswordRequired = "User.NewPasswordRequired";
        public const string NewPasswordLength = "User.NewPasswordLength";
        public const string NewPasswordFormat = "User.NewPasswordFormat";
        public const string ConfirmPasswordRequired = "User.ConfirmPasswordRequired";
        public const string PasswordsDoNotMatch = "User.PasswordsDoNotMatch";
        public const string EmailRequired = "User.EmailRequired";
        public const string EmailInvalid = "User.EmailInvalid";
        public const string PasswordRequired = "User.PasswordRequired";
        public const string FirstNameRequired = "User.FirstNameRequired";
        public const string LastNameRequired = "User.LastNameRequired";
        public const string PhoneNumberRequired = "User.PhoneNumberRequired";
        public const string PhoneNumberInvalid = "User.PhoneNumberInvalid";
    }

    public static class RefreshToken
    {
        public const string TokenRequired = "RefreshToken.TokenRequired";
        public const string UserIdRequired = "RefreshToken.UserIdRequired";
        public const string ClientIdRequired = "RefreshToken.ClientIdRequired";
        public const string Invalid = "RefreshToken.Invalid";
        public const string Expired = "RefreshToken.Expired";
        public const string Revoked = "RefreshToken.Revoked";
    }

    public static class Client
    {
        public const string NotFound = "Client.NotFound";
        public const string ClientIdRequired = "Client.ClientIdRequired";
        public const string NameRequired = "Client.NameRequired";
        public const string UrlRequired = "Client.UrlRequired";
        public const string UrlInvalid = "Client.UrlInvalid";
    }
}