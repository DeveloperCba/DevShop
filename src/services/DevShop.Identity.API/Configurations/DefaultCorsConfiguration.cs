namespace DevShop.Identity.API.Configurations;

public static class DefaultCorsConfiguration
{
    public static IServiceCollection AddDefaultCorsConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        try
        {
            Console.WriteLine("Adicionando configurações de CORS.");
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                        builder.WithOrigins(configuration["CORS"].Split(","))
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                );
            });
            Console.WriteLine("Configurações de CORS adicionadas com sucesso.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao adicioar as configurações de cors. Erro: \n {e}.");
            throw;
        }

        return services;
    }

    public static void UseCorsCustom(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseCors(builder =>
            builder.WithOrigins(configuration["CORS"].Split(","))
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
    }
}