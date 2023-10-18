using System.Text.Json.Serialization;
using DevShop.Core.Helpers;
using DevShop.WebAPI.Core.Identities;

namespace DevShop.Identity.API.Configurations;

public static class ApiConfiguration
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new CustomTimeSpanConverter());
            options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter());
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        services.AddVersioningConfiguration();

        services.AddDefaultCorsConfigurations(configuration);

        return services;
    }

    public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
    {
        if (env.IsDevelopment())
        {
            //app.UseDeveloperExceptionPage();
        }
           
        app.UseCorsCustom(configuration);

        app.UseStaticFiles();
        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseGlobalizationConfig();

        app.UseAuthConfiguration();

        //app.UseRequestResponseLoggingMiddleware();

        //if(env.IsProduction())
          app.UseExceptionMiddleware();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return app;
    }
}