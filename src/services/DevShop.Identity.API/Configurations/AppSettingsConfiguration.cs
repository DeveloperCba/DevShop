using DevShop.Core.Comunications;
using DevShop.Core.Validations.Services.Email;
using DevShop.Identity.Application.Models;

namespace DevShop.Identity.API.Configurations;

public static class AppSettingsConfiguration
{

    public static IServiceCollection AddConfigurationParameter(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var emailSettingsSection = configuration.GetSection(nameof(EmailSettings));
        services.Configure<EmailSettings>(emailSettingsSection);
        var emailSettings = emailSettingsSection.Get<EmailSettings>();
   
        var externalEmailSettingsSection = configuration.GetSection(nameof(ExternalEmailSettings));
        services.Configure<ExternalEmailSettings>(externalEmailSettingsSection);
        var externalEmailSettings = externalEmailSettingsSection.Get<ExternalEmailSettings>();

        var redisSettingsSection = configuration.GetSection(nameof(RedisSettings));
        services.Configure<RedisSettings>(redisSettingsSection);

        var jwtSettingsSection = configuration.GetSection(nameof(JwtSettings));
        services.Configure<JwtSettings>(jwtSettingsSection);

        return services;
           
    }
}