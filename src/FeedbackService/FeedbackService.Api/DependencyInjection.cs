using FeedbackService.Api;
using FeedbackService.Api.Transformers;
using MassTransit;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OrderService.Application;
using OrderService.Infrastructure;
using RabbitMQ.Client;
using Serilog;
using SharedKernel;

namespace FeedbackService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddFeedbackServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
                .AddSharedKernelServices(configuration)
                .AddApiDocumentation()
                .AddApplicationServices()
                .AddInfrastructureServices(configuration)
                .AddHealthChecksService(configuration)
                .AddOpenTelemetryDistributedTracing();

            


        return services;
    }

    private static IServiceCollection AddApiDocumentation(this IServiceCollection services)
    {

        services.AddOpenApi("v1", options =>
        {
            // Use transformer class
            options.AddDocumentTransformer<DefaultInfoTransformer>();


            // Security Scheme config
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            options.AddOperationTransformer<BearerSecuritySchemeTransformer>();
        });

        return services;
    }


    private static IServiceCollection AddHealthChecksService(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitConnectionString =
                        $"amqp://{configuration["MessageBroker:Username"]}:{configuration["MessageBroker:Password"]}@{configuration["MessageBroker:Host"]}/";

        services.AddHealthChecks()
                .AddSqlServer(
                    connectionString: configuration.GetConnectionString("FeedbackConnection")!,
                    name: "SQL Server",
                    tags: new[] { "database", "critical" })

                .AddRedis(
                    redisConnectionString: configuration.GetConnectionString("Redis")!,
                    name: "Redis Cache",
                    tags: new[] { "cache", "critical" })

                .AddRabbitMQ(async sp =>
                {
                    var factory = new ConnectionFactory
                    {
                        HostName = configuration["MessageBroker:Host"]!,
                        UserName = configuration["MessageBroker:Username"]!,
                        Password = configuration["MessageBroker:Password"]!,
                        Port = 5672
                    };

                    return await factory.CreateConnectionAsync();
                },
                    name: "RabbitMQ",
                    tags: new[] { "messagebroker", "critical" });

        return services;
    }

    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, loggerCongiguration) =>
            loggerCongiguration.ReadFrom.Configuration(context.Configuration));

        return builder;
    }

    private static IServiceCollection AddOpenTelemetryDistributedTracing(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
                .ConfigureResource(res => res.AddService("feedbackservice"))
                .WithTracing(tracing =>
                {
                    tracing.AddAspNetCoreInstrumentation().
                    AddHttpClientInstrumentation();
                    tracing.AddOtlpExporter();
                })
                .WithMetrics(metrics =>
                {
                    metrics.AddAspNetCoreInstrumentation().
                    AddHttpClientInstrumentation();
                    metrics.AddOtlpExporter().
                    AddPrometheusExporter(); // Prometheus and Grafana
                });

        return services;
    }


    

   

}
