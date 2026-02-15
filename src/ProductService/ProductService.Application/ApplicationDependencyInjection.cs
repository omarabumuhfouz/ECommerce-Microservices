using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel;
using System.Reflection;

namespace ProductService.Infrastructure
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddProductApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
            .AddAutoMapperService(AssemblyReference.Assembly)
            .AddFluentValidationService(AssemblyReference.Assembly)
            .AddMediatRService(AssemblyReference.Assembly);
            
            return services;
        }
    }
}
