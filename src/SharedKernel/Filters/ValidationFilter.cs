using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Filters;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        // 1. Get the Validator and the Model
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        var model = context.Arguments.OfType<T>().FirstOrDefault();

        // 2. Check if Validator or Model exists
        // If not, we return a generic "Problem" (Status 400)
        if (validator is null || model is null)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Validation Error",
                detail: $"Could not find a validator or model for type {nameof(T)}."
            );
        }

        // 3. Validate
        var result = await validator.ValidateAsync(model);

        if (!result.IsValid)
        {
            // 4. Convert to Dictionary<string, string[]>
            // This is the format Results.ValidationProblem expects
            var errors = result.Errors
                .GroupBy(g => g.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).ToArray()
                );

            // 5. Return Standard ProblemDetails
            // This automatically sets type, title, status(400), and the "errors" object
            return Results.ValidationProblem(errors);
        }

        return await next(context);
    }
}
