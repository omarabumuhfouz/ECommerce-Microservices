namespace FrontEnd_Ecommerce.Constaints;

public static class MessageType
{
    public const string Success = "Success";
    public const string Error = "Error";
    public const string Info = "Info";
    public const string Warning = "Warning";

    // // For Futher usings
    // public static string GetCssClass(string type) => type switch
    // {
    //     Success => "alert-success",
    //     Error => "alert-danger",
    //     Info => "alert-info",
    //     Warning => "alert-warning",
    //     _ => "alert-secondary"
    // };
}
