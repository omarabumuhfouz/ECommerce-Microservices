using AuthService.Api.Endpoints;
using AuthService.Endpoints;
using AuthService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SharedKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.AddSerilogLogging();

var app = builder.Build();
app.UseRouting();

app.UseSharedApplicationMiddleware();
app.MapSharedDevelopmentEndpoints();

app.MapAuthEndpoints();
app.MapJwksEndpoints();
app.MapHealthCheckEndpoints();
 

// Serilog Configuration
app.UseSerilogRequestLogging();

// Prometheus
app.UseOpenTelemetryPrometheusScrapingEndpoint();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        
        if (context.Database.IsRelational())
        {
            await context.Database.MigrateAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
        throw;
    }
}

await SeedData.SeedAsync(app.Services);

app.Run();
