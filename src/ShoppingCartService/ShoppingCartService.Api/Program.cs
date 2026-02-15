using Serilog;
using SharedKernel;
using ShoppingCartService.Api;
using ShoppingCartService.Api.Endpoints;
using ShoppingCartService.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.AddSerilogLogging();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CartDbContext>();
        await CartDataSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}


app.UseRouting();

app.MapCartsEndpoints();

app.UseSharedApplicationMiddleware();
app.MapSharedDevelopmentEndpoints();


app.UseSerilogRequestLogging();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.MapHealthCheckEndpoints();


app.Run();

