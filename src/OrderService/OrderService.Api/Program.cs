using OrderService.Api;
using OrderService.Api.Endpoints;
using OrderService.Api.Services;
using OrderService.Infrastructure.Data;
using Scalar.AspNetCore;
using Serilog;
using SharedKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOrderServices(builder.Configuration);
builder.AddSerilogLogging();

var app = builder.Build();

app.UseRouting();

app.MapOrdersEndpoints();
app.UseSharedApplicationMiddleware();
app.MapSharedDevelopmentEndpoints();

try
{
    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        
        // IMPORTANT: Replace 'ApplicationDbContext' with your actual DbContext class name
        var dbContext = serviceProvider.GetRequiredService<OrderDbContext>(); 
        
        await DataSeeder.SeedAsync(dbContext);
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during data seeding.");
}

app.MapGrpcService<GrpcOrderServer>();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.UseSerilogRequestLogging();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.MapHealthCheckEndpoints();

app.Run();
