using AuthService.Domain.Extensions;
using AuthService.Domain.Helpers;

namespace AuthService.Domain.Users;

public record User : AggregateRoot
{
    private User() { }

    private User(Guid id, string email, string passwordHash, int roles, DateTime createdAt) : base(id)
    {
        Email = email;
        PasswordHash = passwordHash;
        Roles = roles;
        CreatedAt = createdAt;
    }

    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public int Roles { get; private set; }
    public DateTime CreatedAt { get; private set; }


    public static Result<User> Create(string email, string passwordHash, List<UserRole> roles)
    {
        if (!email.IsValidEmail()) return DomainErrors.User.EmailInvalid;

        if (string.IsNullOrWhiteSpace(passwordHash)) return DomainErrors.User.PasswordRequired;

        var user = new User(
            Guid.NewGuid(),
            email,
            passwordHash,
            UserRoleHelper.GetRolesAsInt(roles),
            DateTime.UtcNow
        );

        return Result.Success(user);
    }

    public Result ChangeEmail(string newEmail)
    {
        if (!newEmail.IsValidEmail()) return DomainErrors.User.EmailInvalid;

        Email = newEmail;
        return Result.Success();
    }

    public Result ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash)) return DomainErrors.User.PasswordRequired;

        PasswordHash = newPasswordHash;
        return Result.Success();
    }

    public Result ChangeRoles(List<UserRole> roles)
    {
        if (roles is null || roles.Count == 0) return DomainErrors.User.RolesRequired;

        Roles = UserRoleHelper.GetRolesAsInt(roles);
        return Result.Success();
    }


}
