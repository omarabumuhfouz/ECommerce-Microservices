// Infrastructure/Localization/LocalizationKeys.cs
public static class LocalizationKeys
{
    public static class Auth
    {
        public const string Login = "";
        public const string Register = "";
        public const string Logout = "";
        public const string ForgotPassword = "";
        public const string ResetPassword = "";
        public const string ChangePassword = "";
        public const string WelcomeMessage = "";
        public const string RegisterSuccess = "";
        public const string RegisterFailed = "";
        public const string LogoutSuccess = "";
        public const string LogoutFailed = "";
        public const string LoginFailed = "";
        public const string ChangePasswordSuccess = "";
    }

    public static class Common
    {
        public const string Save = "common_save";
        public const string Cancel = "common_cancel";
        public const string Delete = "common_delete";
        public const string Edit = "common_edit";
        public const string Loading = "common_loading";
        public const string Success = "common_success";
        public const string Error = "common_error";
    }

    public static class Validation
    {
        public const string RequiredField = "validation_required_field";
        public const string InvalidEmail = "validation_invalid_email";
        public const string MinLength = "validation_min_length";
        public const string PasswordsNotMatch = "validation_passwords_not_match";
    }

    public static class Profile
    {
        public const string Title = "profile_title";
        public const string FirstName = "profile_first_name";
        public const string LastName = "profile_last_name";
        public const string Email = "profile_email";
        public const string Phone = "profile_phone";
        public const string UpdateSuccess = "profile_update_success";
        public const string UpdateError = "profile_update_error";
    }

    public static class Orders
    {
        public const string Title = "orders_title";
        public const string TotalOrders = "orders_total_orders";
        public const string RecentOrders = "orders_recent_orders";
        public const string OrderNumber = "orders_order_number";
        public const string OrderDate = "orders_order_date";
        public const string OrderStatus = "orders_order_status";
    }

    public static class Navigation
    {
        public const string Home = "navigation_home";
        public const string Profile = "navigation_profile";
        public const string Orders = "navigation_orders";
        public const string Settings = "navigation_settings";
    }
}