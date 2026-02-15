using FeedbackService.Api;
using FeedbackService.Api.Endpoints;
using FeedbackService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SharedKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFeedbackServices(builder.Configuration);
builder.AddSerilogLogging();

var app = builder.Build();

app.UseRouting();

app.MapFeedbacksEndpoints();
app.UseSharedApplicationMiddleware();
app.MapSharedDevelopmentEndpoints();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FeedbackDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<FeedbackDataSeeder>>();
    
    // Automatically apply migrations first
    await context.Database.MigrateAsync();
    
    var seeder = new FeedbackDataSeeder(context, logger);
    await seeder.SeedAsync();
}


// if (app.Environment.IsDevelopment())
// {
//     app.MapGrpcReflectionService();
// }

app.UseSerilogRequestLogging();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.MapHealthCheckEndpoints();

app.Run();

