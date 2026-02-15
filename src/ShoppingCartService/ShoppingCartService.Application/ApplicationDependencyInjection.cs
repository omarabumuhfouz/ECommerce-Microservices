using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace ShoppingCartService.Application
{
   public static class ApplicationDependencyInjection
   {
      public static IServiceCollection AddCartsApplicationServices(this IServiceCollection services, IConfiguration configuration)
      {

         var assembly = Assembly.GetExecutingAssembly();

         services
            .AddBusinessService()
            .AddAutoMapperService(assembly)
            .AddFluentValidationService(assembly)
            .AddMediatrService(assembly);

         return services;
      }

      private static IServiceCollection AddBusinessService(this IServiceCollection services)
      {
         services.AddScoped<IValidationService, ValidationService>();

         return services;
      }

      private static IServiceCollection AddMediatrService(this IServiceCollection services, Assembly assembly)
      {
         services.AddMediatR(cfg =>
         {
            cfg.RegisterServicesFromAssembly(assembly);

            // cfg.AddOpenBehavior(typeof(IdempotentBehavior<,>));
         });

         return services;
      }
   }
}
