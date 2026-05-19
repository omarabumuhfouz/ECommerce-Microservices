namespace AuthService.Domain.Errors;

public static class DomainErrors
{
    public static class User
    {
public static Error EmailAlreadyExists => Error.Conflict(
            ErrorCodes.User.EmailAlreadyExists,
            "The provided email address is already in use.");
public static Error EmailInvalid => Error.Validation(
            ErrorCodes.User.EmailInvalid, 
            "The provided email format is invalid.");

        public static Error PasswordRequired => Error.Validation(
            ErrorCodes.User.PasswordRequired, 
            "The password cannot be empty.");

        public static Error RolesRequired => Error.Validation(
            ErrorCodes.User.RolesRequired, 
            "The user must be assigned at least one role.");

        public static Error RoleInvalid => Error.Validation(
            ErrorCodes.User.RoleInvalid,
            "The provided role is invalid.");
        public static Error NotFound(Guid id) => Error.NotFound(
            ErrorCodes.User.NotFound,
            $"User with Id '{id}' was not found.");

        public static Error EmailNotUnique => Error.Conflict(
            ErrorCodes.User.EmailNotUnique,
            "The provided email is already in use.");

        public static Error InvalidCredentials => Error.Unauthorized(
            ErrorCodes.User.InvalidCredentials,
            "Invalid email or password.");

        public static Error NotActive => Error.Conflict(
            ErrorCodes.User.NotActive,
            "The user account is not active.");

        public static Error AlreadyVerified => Error.Conflict(
            ErrorCodes.User.AlreadyVerified,
            "The user account has already been verified.");
    }

    public static class RefreshToken
    {
        public static Error TokenRequired => Error.Validation(
            ErrorCodes.RefreshToken.TokenRequired,
            "The refresh token string cannot be empty.");

        public static Error UserIdRequired => Error.Validation(
            ErrorCodes.RefreshToken.UserIdRequired,
            "The User Id is required to create a refresh token.");

        public static Error ClientIdRequired => Error.Validation(
            ErrorCodes.RefreshToken.ClientIdRequired,
            "The Client Id is required to create a refresh token.");

        public static Error Invalid => Error.Validation(
            ErrorCodes.RefreshToken.Invalid,
            "The provided refresh token is invalid.");

        public static Error Expired => Error.Validation(
            ErrorCodes.RefreshToken.Expired,
            "The refresh token has expired.");

        public static Error Revoked => Error.Conflict(
            ErrorCodes.RefreshToken.Revoked,
            "The refresh token has been revoked.");
    }

    public static class Client
    {
        public static Error ClientIdRequired => Error.Validation(
            ErrorCodes.Client.ClientIdRequired,
            "The Client Identifier is required.");

        public static Error NameRequired => Error.Validation(
            ErrorCodes.Client.NameRequired,
            "The Client Name is required.");

        public static Error UrlRequired => Error.Validation(
            ErrorCodes.Client.UrlRequired,
            "The Client URL is required.");

        public static Error UrlInvalid => Error.Validation(
            ErrorCodes.Client.UrlInvalid,
            "The Client URL format is invalid.");

        public static Error NotFound => Error.NotFound(
            ErrorCodes.Client.NotFound, "The provided Client ID was not found.");

    }

}