using AuthService.Domain.Users;

namespace AuthService.Domain.Helpers;

public static class UserRoleHelper
{
    // Check if user has a specific role
    public static bool HasRole(int userRoles, UserRole role)
    {
        return ((UserRole)userRoles & role) == role;
    }

    // Add a role to a user
    public static int AddRole(int userRoles, UserRole role)
    {
        return userRoles |= (int)role;
    }

    // Remove a role from a user
    public static int RemoveRole(int userRoles, UserRole role)
    {
        return userRoles &= ~(int)role;
    }

    // Get all roles as strings
    public static List<string> GetRolesAsStrings(int userRoles)
    {
        var roles = new List<string>();
        foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
        {
            if (role != UserRole.None && HasRole(userRoles, role))
                roles.Add(role.ToString());
        }

        foreach (var role in roles)
            System.Console.WriteLine($"Role : {role}");

        return roles;
    }

    // Get all roles as a list
    public static List<UserRole> GetRoles(int userRoles)
    {
        var roles = new List<UserRole>();
        foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
        {
            if (role != UserRole.None && HasRole(userRoles, role))
                roles.Add(role);
        }
        return roles;
    }

    public static int GetRolesAsInt(List<UserRole> roles)
    {
        return roles.Aggregate(0, (cuurent, role) => cuurent | (int)role);
    }
}
