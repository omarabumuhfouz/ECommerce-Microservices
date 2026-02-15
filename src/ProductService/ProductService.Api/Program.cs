using CustomerService.Api.Endpoints;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using ProductService.Api;
using ProductService.Api.Endpoints;
using ProductService.Api.Services;
using ProductService.Infrastructure.Data;
using Serilog;
using SharedKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.AddSerilogLogging();

// builder.WebHost.ConfigureKestrel(options =>
// {
//     // Port for REST / HTTP1.1
//     options.ListenAnyIP(7002, listenOptions =>
//     {
//         listenOptions.Protocols = HttpProtocols.Http1;
//     });

//     // Port for gRPC / HTTP2
//     options.ListenAnyIP(5002, listenOptions =>
//     {
//         listenOptions.Protocols = HttpProtocols.Http2;
//     });
// });

var app = builder.Build();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService().AllowAnonymous();    
}

app.MapTagsEndpoints();
app.MapProductsEndpoints();
app.MapCategoriesEndpoints();

app.UseSharedApplicationMiddleware();
app.MapSharedDevelopmentEndpoints();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var dbContext = services.GetRequiredService<ProductDbContext>();

        // 1. (Recommended) Apply migrations automatically
        logger.LogInformation("Applying database migrations...");
        await dbContext.Database.MigrateAsync();

        // 2. Run your seeder
        await DatabaseSeeder.SeedDatabaseAsync(dbContext, logger);
    }
    catch (Exception ex)
    {
        // Log the error if migrations or seeding fail
        logger.LogError(ex, "An error occurred during database initialization.");
    }
}






app.MapGrpcService<ProductGrpcService>();

// Serilog Configuration
app.UseSerilogRequestLogging();

// Prometheus
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.MapHealthCheckEndpoints();

app.Run();
