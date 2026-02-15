using FrontEnd_Ecommerce.Middlewares;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseApplicationMiddlewares(this IApplicationBuilder app)
    {

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        // Add Localication
        var supportedCultures = new[]
        {
            "en-US",
            "ar-JO",
        };

        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);

        app.UseRequestLocalization(localizationOptions);

    


        return app;
    }
}
