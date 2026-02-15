using Ardalis.SmartEnum.SystemTextJson;
using MassTransit;
using Microsoft.AspNetCore.Http.Json;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using PaymentService.Api.Consumers;
using PaymentService.Application;
using PaymentService.Domain.Payments.Enums;
using PaymentService.Infrastructure;
using ProductService.Api.OpenApi.Transformers;
using RabbitMQ.Client;
using Serilog;
using SharedKernel;

namespace ProductService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPaymentServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
        .AddSharedKernelServices(configuration)
                .AddApiDocumentation()
                .AddApplicationServices()
                .AddInfrastructureServices(configuration)
                .AddHealthChecksService(configuration)
                .AddOpenTelemetryDistributedTracing()
                .AddMassTransitService()
                .AddJsonOptions();


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

    private static IServiceCollection AddJsonOptions(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new SmartEnumNameConverter<PaymentStatus, int>());
            options.SerializerOptions.Converters.Add(new SmartEnumNameConverter<PaymentMethod, int>());
            
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        });

        return services;
    }


private static IServiceCollection AddHealthChecksService(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitConnectionString =
                        $"amqp://{configuration["MessageBroker:Username"]}:{configuration["MessageBroker:Password"]}@{configuration["MessageBroker:Host"]}/";

        services.AddHealthChecks()
                .AddSqlServer(
                    connectionString: configuration.GetConnectionString("PaymentConnection")!,
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
        // Now you can access 'builder.Host'
        builder.Host.UseSerilog((context, loggerCongiguration) =>
            loggerCongiguration.ReadFrom.Configuration(context.Configuration));

        return builder;
    }

    private static IServiceCollection AddOpenTelemetryDistributedTracing(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
                .ConfigureResource(res => res.AddService("paymentservice"))
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

    private static IServiceCollection AddMassTransitService(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<InitiateRefundOnCancellationApprovedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("payment-service-cancellation-approved", e =>
                {
                    e.ConfigureConsumer<InitiateRefundOnCancellationApprovedConsumer>(context);
                });
            });
        });
        return services;
    }



}
