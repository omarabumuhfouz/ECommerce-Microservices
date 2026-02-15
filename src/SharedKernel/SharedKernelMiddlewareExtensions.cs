using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Middlewares;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;
namespace SharedKernel;

public static class SharedKernelMiddlewareExtensions
{
    public static IApplicationBuilder UseSharedApplicationMiddleware(this IApplicationBuilder app)
    {
        var supportedCultures = new[]
        {
            "en-US",
            "ar-JO",
        };

        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures);

        app.UseRequestLocalization(localizationOptions);

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }


    public static IApplicationBuilder MapSharedDevelopmentEndpoints(this IApplicationBuilder app)
    {
        var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

        if (env.IsDevelopment())
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapOpenApi();
                endpoints.MapScalarApiReference();
            });
        }
        
        return app;
    }


}