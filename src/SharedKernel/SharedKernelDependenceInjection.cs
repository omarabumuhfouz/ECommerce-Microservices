using System.Reflection;
using Contracts.Cart;
using Contracts.Customer;
using Contracts.Order;
using Contracts.Product;
using Contracts.User;
using FluentValidation;
using MechanicShop.Application.Common.Behaviours;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Abstractions;
using SharedKernel.Behaviors;
using SharedKernel.Constants;
using SharedKernel.Repositories;
using SharedKernel.Services;

namespace SharedKernel;

public static class SharedKernelDependenceInjection
{
    /// <summary>
    /// Adds MediatR and shared pipeline behaviors to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="appAssembly">
    /// The assembly to scan for MediatR handlers (e.g., your OrderService.Application assembly).
    /// </param>
    public static IServiceCollection AddSharedKernelServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddSharedJwtAuthentication(configuration)
            .AddAuthorization()
            .AddSharedBusineesServices()
            .AddSharedPipelineBehaviors();



        return services;
    }

    #region Private Static Services
    private static IServiceCollection AddSharedBusineesServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
services.AddHttpClient();
services.AddMemoryCache();
services.AddSingleton<JwksKeyProvider>();
        return services;
    }

    private static IServiceCollection AddSharedPipelineBehaviors(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(InvalidateCacheBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));


        return services;
    }

    public static IServiceCollection AddSharedJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                // This indicates the authentication scheme that will be used by default when the app attempts to authenticate a user.
                // Which authentication handler to use for verifying who the user is by default.
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                // This indicates the authentication scheme that will be used by default when the app encounters an authentication challenge. 
                // Which authentication handler to use for responding to failed authentication or authorization attempts.
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) => 
                 services.BuildServiceProvider().GetRequiredService<JwksKeyProvider>().GetKeys(token,securityToken,kid,validationParameters)
                };
            });

        services.AddAuthorization(options =>
        {
            // 🛡️ Admin Only
            options.AddPolicy(AuthConstants.Policies.AdminOnly, policy =>
                     policy.RequireRole(AuthConstants.Roles.Admin));

            // 🛍️ Customer Only
            options.AddPolicy(AuthConstants.Policies.CustomerOnly, policy =>
                    policy.RequireRole(AuthConstants.Roles.Customer));

            // 🤝 Shared
            options.AddPolicy(AuthConstants.Policies.Shared, policy =>
                    policy.RequireRole(AuthConstants.Roles.Admin, AuthConstants.Roles.Customer));
        });


        return services;
    }

    #endregion


    #region  Public Static Services
    public static IServiceCollection AddMediatRService(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(assembly));


        return services;

    }

    public static IServiceCollection AddAutoMapperService(this IServiceCollection services, Assembly assembly)
{
    // Fix: Pass an empty config action as the first parameter.
    // This removes ambiguity and forces the compiler to accept the assembly array.
    services.AddAutoMapper(_ => { }, new[] { assembly });

    return services;
}

    public static IServiceCollection AddFluentValidationService(this IServiceCollection services, Assembly assembly)
    {

        services.AddValidatorsFromAssembly(assembly);


        return services;
    }

    public static IServiceCollection AddCustomerGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        string customerServiceUrl = configuration["ServiceUrls:CustomerService"]
        ?? throw new Exception("There is not any Url for Customer Service");


        services.AddGrpcClient<CustomerProtoService.CustomerProtoServiceClient>(
            options =>
        {
            options.Address = new Uri(customerServiceUrl);
        });

        return services;
    }

    public static IServiceCollection AddOrderGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        string orderServiceUrl = configuration["ServiceUrls:OrderService"]
        ?? throw new Exception("There is not any Url for Order Service");


        services.AddGrpcClient<OrderProtoService.OrderProtoServiceClient>(
            options =>
        {
            options.Address = new Uri(orderServiceUrl);
        });

        return services;
    }

   public static IServiceCollection AddUserGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        string userServiceUrl = configuration["ServiceUrls:AuthService"]
        ?? throw new Exception("There is not any Url for Auth Service");


        services.AddGrpcClient<UserProtoService.UserProtoServiceClient>(
            options =>
        {
            options.Address = new Uri(userServiceUrl);
        });

        return services;
    }

    public static IServiceCollection AddProductGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        string userServiceUrl = configuration["ServiceUrls:ProductService"]
        ?? throw new Exception("There is not any Url for Product Service");


        services.AddGrpcClient<ProductProtoService.ProductProtoServiceClient>(
            options =>
        {
            options.Address = new Uri(userServiceUrl);
        });

        return services;
    }

    public static IServiceCollection AddGrpcCartClient(this IServiceCollection services, IConfiguration configuration)
    {
        string userServiceUrl = configuration["ServiceUrls:CartService"]
        ?? throw new Exception("There is not any Url for Cart Service");


        services.AddGrpcClient<CartProtoService.CartProtoServiceClient>(
            options =>
        {
            options.Address = new Uri(userServiceUrl);
        });

        return services;
    }

     #endregion 

}