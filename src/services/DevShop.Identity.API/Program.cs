using DevShop.Core.Datas.Interfaces;
using DevShop.Identity.API.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Configure Services


        builder.Configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddDependencyInjection(builder.Configuration);

        builder.Services.AddAutoMapper(typeof(Program));

        builder.Services.AddApiConfiguration(builder.Configuration);

        //builder.Services.AddIdentityConfiguration(builder.Configuration);

        builder.Services.AddSwaggerConfiguration();



        var app = builder.Build();

        #endregion


        #region Configure Pipeline

        var provider = app.Services.GetService<IApiVersionDescriptionProvider>();

        InitializeDatabase(app);

        app.UseSwaggerConfiguration(provider);

        app.UseApiConfiguration(builder.Environment, builder.Configuration);

        app.Run();

        #endregion
    }

    static void InitializeDatabase(WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();
        dbInitializer!.Initialize();
    }
}

 