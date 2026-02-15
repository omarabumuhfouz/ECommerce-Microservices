using ApiGateway.Endpoints;
using Contracts.Customer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using SharedKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Host.UseSerilog((context, loggerCongiguration) =>
            loggerCongiguration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddOpenTelemetry()
                .ConfigureResource(res => res.AddService("api-gateway"))
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation().
        AddHttpClientInstrumentation();

        tracing.AddOtlpExporter();
    })

    // Add Metrics 
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation().
        AddHttpClientInstrumentation();

        metrics.AddOtlpExporter().
        AddPrometheusExporter(); // Prometheus and Grafana
    });





builder.Services.AddSharedJwtAuthentication(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:9090")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddCustomerGrpcClient(builder.Configuration);
builder.Services.AddOrderGrpcClient(builder.Configuration);
builder.Services.AddUserGrpcClient(builder.Configuration);


var app = builder.Build();
// Serilog Configuration
app.UseSerilogRequestLogging();

// Prometheus
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseHealthCheckEndpoints();


app.UseRouting();
app.UseCors("AllowFrontend");  
app.UseAuthentication();       
app.UseHttpsRedirection();     

await app.UseOcelot();         

app.Run();
