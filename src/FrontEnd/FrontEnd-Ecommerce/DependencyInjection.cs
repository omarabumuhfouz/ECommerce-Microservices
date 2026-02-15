using System.Globalization;
using FluentValidation;
using FluentValidation.AspNetCore;
using FrontEnd_Ecommerce.Common.Filters;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

namespace FrontEnd_Ecommerce;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBussniesServices()
                .AddHttpClients(configuration)
                .AddFilters()
                .AddStringLocalization()
                .AddValidations()
                .AddControllersWithViews();

        


        return services;
    }

    private static IServiceCollection AddBussniesServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRequestRetryHandler, RequestRetryHandler>();
        services.AddTransient<ICookieManager, CookieManager>();
        services.AddScoped(typeof(ApiRequestHandler));
        services.AddScoped<ICategoryService, DummyCategoryService>();
        services.AddScoped<ICartService, DummyCartService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped(typeof(JwtHelper));


        return services;
    }
    private static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("ApiGateway", client =>
        {
            client.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"]!);
        });

        return services;
    }

    private static IServiceCollection AddStringLocalization(this IServiceCollection services)
    {
        services.AddLocalization();

        services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

        services.AddMvc()
            .AddViewLocalization()
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    // Use the JsonStringLocalizer for data annotations
                    return factory.Create(typeof(JsonStringLocalizerFactory));
                };
            });


        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                     new CultureInfo("en-US"),
                     new CultureInfo("ar-JO"),
            };

            options.DefaultRequestCulture = new RequestCulture(culture: supportedCultures[0]);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });

        return services;
    }


    private static IServiceCollection AddValidations(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Program>();
        services.AddFluentValidationAutoValidation();

        return services;
    }

private static IServiceCollection AddFilters(this IServiceCollection services)
    {
        services.AddControllersWithViews(options =>
        {
            // Add your ApiExceptionFilter globally
            options.Filters.Add<ApiExceptionFilter>();
        });

        return services;
    }

}

