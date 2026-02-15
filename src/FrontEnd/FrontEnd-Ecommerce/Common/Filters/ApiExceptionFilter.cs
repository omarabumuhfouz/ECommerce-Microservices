using System.Text;
using System.Text.Json;
using FrontEnd_Ecommerce.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FrontEnd_Ecommerce.Common.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var ex = context.Exception;

        context.Result = ex switch
        {
            // 1. The Main Handler: Use Status Code to decide the View
            ApiException apiEx => HandleApiException(context, apiEx),

            // 2. Auth Specifics (If thrown directly by services before API calls)
            MissingTokenException or RefreshTokenOperationException => RedirectToLogin(ex),

            // 3. Technical/Network Errors
            HttpRequestException httpEx => ErrorView(context, "NetworkError", "Unable to connect to the server.", LogLevel.Warning, httpEx),
            JsonException jsonEx => ErrorView(context, "DataError", "Data processing error.", LogLevel.Error, jsonEx),

            _ => HandleUnknown(context, ex)
        };

        context.ExceptionHandled = true;
    }

    private IActionResult HandleApiException(ExceptionContext context, ApiException ex)
    {
        // Extract the error message (use ResponseContent if available, otherwise exception message)
        // You might want to parse 'ResponseContent' here if it contains JSON 'ProblemDetails'
        var userMessage = ExtractUserMessage(ex);

        return ex.StatusCode switch
        {
            // Auth Errors -> Login Page
            401 => RedirectToLogin(ex),

            // Not Found -> Specific 404 View
            404 => ErrorView(context, "NotFound", userMessage, LogLevel.Information, ex),

            // Bad Request / Validation -> Generic Message View
            400 => ErrorView(context, "Message", userMessage, LogLevel.Information, ex),

            // Conflict (e.g. Duplicate Email) -> Generic Message View
            409 => ErrorView(context, "Message", userMessage, LogLevel.Information, ex),

            // Server Errors -> 500 Page
            >= 500 => ErrorView(context, "InternalServer", "A server error occurred. Please try again later.", LogLevel.Error, ex),

            // Default fallback
            _ => ErrorView(context, "Message", userMessage, LogLevel.Error, ex)
        };
    }

    // 🔹 Helper: Extract a clean message for the user
    private string ExtractUserMessage(ApiException ex)
    {
        if (string.IsNullOrWhiteSpace(ex.ResponseContent))
            return ex.Message;

        // Optional: Try to parse "detail" or "title" from ProblemDetails JSON if your API returns that
        try
        {
            using var doc = JsonDocument.Parse(ex.ResponseContent);
            if (doc.RootElement.TryGetProperty("detail", out var detail)) return detail.GetString()!;
            if (doc.RootElement.TryGetProperty("title", out var title)) return title.GetString()!;
        }
        catch
        {
            // If parsing fails (it's just plain text), return raw content
        }

        return ex.ResponseContent;
    }

    // 🔹 Helper: Build Error View Result
    private ViewResult ErrorView(ExceptionContext context, string viewName, string message, LogLevel level, Exception ex)
    {
        _logger.Log(level, ex, "API Error: {Message} at {Path}", message, context.HttpContext.Request.Path);

        return new ViewResult
        {
            ViewName = viewName,
            ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
            {
                ["ErrorMessage"] = message,
                ["Timestamp"] = DateTime.UtcNow
            }
        };
    }

    // 🔹 Helper: Redirect to Login
    private IActionResult RedirectToLogin(Exception ex)
    {
        _logger.LogInformation("Redirecting to login due to: {Message}", ex.Message);
        return new RedirectToActionResult("Login", "Auth", null); // Assuming AuthController
    }

    // 🔹 Helper: Fallback
    private IActionResult HandleUnknown(ExceptionContext context, Exception ex)
    {
        _logger.LogError(ex, "Unhandled exception at {Path}", context.HttpContext.Request.Path);
        return new RedirectToActionResult("InternalServer", "Error", null);
    }

private string ExtractBestMessage(ApiException ex)
{
    // 1. Handle Empty/Raw 401 immediately
    if (string.IsNullOrWhiteSpace(ex.ResponseContent))
    {
        if (ex.StatusCode == 401)
            return "Your session has expired. Please login again."; // Friendly default
        
        if (ex.StatusCode == 403)
            return "You do not have permission to perform this action.";

        return ex.Message; // Fallback
    }

    try
    {
        // 2. Try to parse ProblemDetails
        using var doc = JsonDocument.Parse(ex.ResponseContent);
        var root = doc.RootElement;

        // A. Check for "errors" (Validation)
        if (root.TryGetProperty("errors", out var errorsElement))
        {
            var sb = new StringBuilder();
            foreach (var property in errorsElement.EnumerateObject())
            {
                foreach (var error in property.Value.EnumerateArray())
                {
                    sb.AppendLine($"• {error.GetString()}");
                }
            }
            if (sb.Length > 0) return sb.ToString();
        }

        // B. Check for "detail" (Developer Message)
        if (root.TryGetProperty("detail", out var detail) && !string.IsNullOrWhiteSpace(detail.GetString()))
        {
            return detail.GetString()!;
        }

        // C. Check for "title"
        if (root.TryGetProperty("title", out var title) && !string.IsNullOrWhiteSpace(title.GetString()))
        {
            return title.GetString()!;
        }
    }
    catch (JsonException)
    {
        // 3. Parsing Failed? It means the body isn't JSON (maybe HTML error page from IIS/Nginx)
        // In this case, ignore the raw HTML and return a safe default.
        if (ex.StatusCode == 401) return "Session expired.";
        if (ex.StatusCode == 500) return "Server error occurred.";
    }

    return ex.Message;
}
}
