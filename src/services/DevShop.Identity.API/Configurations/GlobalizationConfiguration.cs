using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace DevShop.Identity.API.Configurations;

/// <summary>
/// Classe reponsável por gerenciar o Idioma.
/// </summary>
public static class GlobalizationConfiguration
{

    public static IApplicationBuilder UseGlobalizationConfig(this IApplicationBuilder app)
    {
        var defaultCulture = new CultureInfo("pt-BR");
        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(defaultCulture),
            SupportedCultures = new List<CultureInfo> { defaultCulture },
            SupportedUICultures = new List<CultureInfo> { defaultCulture }
        };

        app.UseRequestLocalization(localizationOptions);
        return app;
    }
}