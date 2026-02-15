using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace PaymentService.Api.Endpoints;

public static class HealthCheckEndpoint
{
    public static void MapHealthCheckEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapHealthChecks("/health/startup", new HealthCheckOptions
        {
            Predicate = (check) => check.Tags.Contains("critical"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = (check) => !check.Tags.Contains("critical"),
        });

        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = (check) => check.Tags.Contains("critical"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }
}