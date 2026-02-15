using AuthService.Api.Endpoints;
using AuthService.Endpoints;
using AuthService.Infrastructure.Data;
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

await SeedData.SeedAsync(app.Services);

app.Run();
