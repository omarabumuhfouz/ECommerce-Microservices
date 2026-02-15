using PaymentService.Api.Endpoints;
using PaymentService.Infrastructure.Data;
using ProductService.Api;
using Serilog;
using SharedKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPaymentServices(builder.Configuration);
builder.AddSerilogLogging();

var app = builder.Build();

app.UseRouting();

app.MapPaymentsEndpoints();
app.UseSharedApplicationMiddleware();
app.MapSharedDevelopmentEndpoints();

try
{
    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        
        // IMPORTANT: Replace 'ApplicationDbContext' with your actual DbContext class name
        var dbContext = serviceProvider.GetRequiredService<PaymentDbContext>(); 
        
        await PaymentDataSeeder.SeedAsync(dbContext);
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during data seeding.");
}

// if (app.Environment.IsDevelopment())
// {
//     app.MapGrpcReflectionService();
// }

app.UseSerilogRequestLogging();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.MapHealthCheckEndpoints();

app.Run();

