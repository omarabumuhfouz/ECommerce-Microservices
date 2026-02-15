using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder; // Change namespace
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace ApiGateway.Endpoints;

public static class HealthCheckEndpoint
{
    public static void UseHealthCheckEndpoints(this IApplicationBuilder app)
    {
        
        app.UseHealthChecks("/health/startup", new HealthCheckOptions
        {
            Predicate = (check) => check.Tags.Contains("critical"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = (check) => !check.Tags.Contains("critical"),
        });

        app.UseHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = (check) => check.Tags.Contains("critical"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }
}