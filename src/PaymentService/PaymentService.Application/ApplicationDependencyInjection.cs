using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace PaymentService.Application
{
   public static class ApplicationDependencyInjection
   {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services
           .AddAutoMapperService(AssemblyReference.Assembly)
            .AddFluentValidationService(AssemblyReference.Assembly)
            .AddMediatRService(AssemblyReference.Assembly)
            .AddBusinessServices(); // Check form this


            return services;
        }

private static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IValidationService, ValidationService>();
        return services;
    }
   }
}