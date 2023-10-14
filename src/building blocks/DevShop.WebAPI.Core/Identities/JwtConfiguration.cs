using System.Text;
using DevShop.Core.Comunications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DevShop.WebAPI.Core.Identities;

public static class JwtConfiguration
{
    public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettingsSection = configuration.GetSection(nameof(JwtSettings));

        services.Configure<JwtSettings>(jwtSettingsSection);

        var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

        var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })  
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;  //Acesso pelo https
                x.SaveToken = true;             //Permitir salvar o token na instância
                //x.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.ValidOn,
                    ValidIssuer = jwtSettings.Issuer
                };
            });
    }

    public static void AddJwtConfiguration(this IServiceCollection services, JwtSettings jwtSettings)
    {
        var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

        //Configuração da autenticação do via JWT.
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;  //Acesso pelo https
                x.SaveToken = true;             //Permitir salvar o token na instância
                x.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.ValidOn,
                    ValidIssuer = jwtSettings.Issuer
                };
            });
    }

    public static void UseAuthConfiguration(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}