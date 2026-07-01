using CancellationService.Api;
using CancellationService.Api.Endpoints;
using Serilog;
using SharedKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCancellationServiceApplicationServices(builder.Configuration);
builder.AddSerilogLogging();

var app = builder.Build();


app.UseRouting();

app.MapCancellationEndpoints();

app.UseSharedApplicationMiddleware();
app.MapSharedDevelopmentEndpoints();



// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;


//     try
//     {
//         await SeedData.SeedAsync(app.Services);
//     }
//     catch (Exception ex)
//     {
//         var logger = services.GetRequiredService<ILogger<Program>>();
//         logger.LogError(ex, "An error occurred during seeding.");
//     }
// }

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

// app.MapGrpcService<GrpcCustomerService>();

// Serilog Configuration
app.UseSerilogRequestLogging();

// Prometheus
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.MapHealthCheckEndpoints();


app.Run();

