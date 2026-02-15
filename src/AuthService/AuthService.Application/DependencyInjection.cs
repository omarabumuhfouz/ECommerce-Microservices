using AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel;
using System.Reflection;

namespace AuthService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
                .AddMediatRService(Assembly.GetExecutingAssembly())
                .AddFluentValidationService(Assembly.GetExecutingAssembly())
                .AddBusinessServices();

        return services;
    }

    private static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordService, PasswordService>();

        return services;
    }
}