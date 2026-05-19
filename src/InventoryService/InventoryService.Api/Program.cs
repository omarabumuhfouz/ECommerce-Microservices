using InventoryService.Api;
using InventoryService.Api.Endpoints;
using Serilog;
using SharedKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInventoryServices(builder.Configuration);
builder.AddSerilogLogging();

var app = builder.Build();

app.UseRouting();

app.MapInventoryEndpoints();
app.UseSharedApplicationMiddleware();
app.MapSharedDevelopmentEndpoints();

// try
// {
//     using (var scope = app.Services.CreateScope())
//     {
//         var serviceProvider = scope.ServiceProvider;
        
//         // IMPORTANT: Replace 'ApplicationDbContext' with your actual DbContext class name
//         var dbContext = serviceProvider.GetRequiredService<InventoryDbContext>(); 
        
//         await DataSeeder.SeedAsync(dbContext);
//     }
// }
// catch (Exception ex)
// {
//     var logger = app.Services.GetRequiredService<ILogger<Program>>();
//     logger.LogError(ex, "An error occurred during data seeding.");
// }


app.UseSerilogRequestLogging();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.MapHealthCheckEndpoints();

app.Run();

