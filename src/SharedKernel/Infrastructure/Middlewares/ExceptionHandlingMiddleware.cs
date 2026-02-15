using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // Required for ProblemDetails
using Microsoft.Extensions.Logging;
using System.Net;

namespace SharedKernel.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next, 
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // 1. Determine the Status Code based on the Exception Type
        // You can add your custom domain exceptions here (e.g., DomainException, NotFoundException)
        int statusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            // If you have a custom ValidationException, add it here:
            // ValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        // 2. Create the ProblemDetails Object
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitle(statusCode),
            Detail = exception.Message,
            Type = GetType(statusCode),
            Instance = context.Request.Path
        };

        // 3. Add Extension Data (Optional)
        // Adding TraceId helps you find the specific log for this error later
        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        // 4. Set Content Type to standard "application/problem+json"
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        // 5. Write JSON
        // WriteAsJsonAsync automatically handles camelCase and serialization settings
        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static string GetTitle(int statusCode) =>
        statusCode switch
        {
            StatusCodes.Status400BadRequest => "Bad Request",
            StatusCodes.Status404NotFound => "Not Found",
            StatusCodes.Status500InternalServerError => "Internal Server Error",
            _ => "An error occurred"
        };
        
    private static string GetType(int statusCode) =>
        statusCode switch
        {
            StatusCodes.Status400BadRequest => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            StatusCodes.Status404NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            StatusCodes.Status500InternalServerError => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            _ => "https://tools.ietf.org/html/rfc7231"
        };
}
