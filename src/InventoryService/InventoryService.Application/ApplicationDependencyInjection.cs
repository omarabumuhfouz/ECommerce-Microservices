using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace InventoryService.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
               .AddAutoMapperService(AssemblyReference.Assembly)
               .AddFluentValidationService(AssemblyReference.Assembly)
               .AddMediatRService(AssemblyReference.Assembly);


        return services;
    }

}
