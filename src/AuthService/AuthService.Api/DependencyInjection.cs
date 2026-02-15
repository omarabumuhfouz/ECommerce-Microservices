using AuthService.Application;
using AuthService.Features.Users.Consumers;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Data;
using AuthService.OpenApi.Transformers;
using Contracts.Customer;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RabbitMQ.Client;
using Serilog;
using SharedKernel;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
                .AddSharedKernelServices(configuration)
                .AddApplication()
                .AddInfrastructure(configuration)
                .AddApiDocumentation()
                .AddGrpcClients(configuration)
                .AddMassTransitService()
                .AddOpenTelemetryDistributedTracing()
                .AddHealthChecksService(configuration)
                .AddHttpContextAccessor()
                .AddGrpc();

        return services;
    }

    private static IServiceCollection AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<CustomerProtoService.CustomerProtoServiceClient>(options =>
        {
            options.Address = new Uri(configuration["ServiceUrls:ProductService"]!);
        });

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

    private static IServiceCollection AddMassTransitService(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<CustomerCreationFailedConsumer>();
            x.AddEntityFrameworkOutbox<AppDbContext>(o =>
            {
                o.UseSqlServer();

                o.UseBusOutbox();
            });

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("auth-customer-creation-failed", e =>
                {
                    e.ConfigureConsumer<CustomerCreationFailedConsumer>(context);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

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
                .ConfigureResource(res => res.AddService("authservice"))
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



        return services;
    }

    private static IServiceCollection AddHealthChecksService(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitConnectionString =
                        $"amqp://{configuration["MessageBroker:Username"]}:{configuration["MessageBroker:Password"]}@{configuration["MessageBroker:Host"]}/";

        services.AddHealthChecks()
                .AddSqlServer(
                    connectionString: configuration.GetConnectionString("AuthServiceDb")!,
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
}