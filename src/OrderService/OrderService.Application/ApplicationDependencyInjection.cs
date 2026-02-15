using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace OrderService.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
               .AddAutoMapperService(AssemblyReference.Assembly)
                .AddFluentValidationService(AssemblyReference.Assembly)
                .AddMediatRService(AssemblyReference.Assembly)
                .AddBusinessServices();


        return services;
    }

    private static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IValidationService, ValidationService>();
        return services;
    }

}
