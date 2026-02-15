namespace SharedKernel.Constants;

public static class AuthConstants
{
    // 🎭 The Roles (Must match what is inside your JWT Token)
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
    }

    // 📜 The Policy Names (Used in Program.cs and Endpoints)
    public static class Policies
    {
        public const string AdminOnly = "AdminPolicy";
        public const string CustomerOnly = "CustomerPolicy";
        public const string Shared = "SharedPolicy";
    }
}