using System.Reflection;
using DevShop.Core.Configurations.Redis;
using DevShop.Core.Datas.Implementations;
using DevShop.Core.Datas.Interfaces;
using DevShop.Core.DomainObjects;
using DevShop.Core.Mediator;
using DevShop.Core.Validations.Services.Email;
using DevShop.Identity.Application.Contracts;
using DevShop.Identity.Application.Features.Auth.Validators;
using DevShop.Identity.Application.Features.User.Validators;
using DevShop.Identity.Application.Mappings;
using DevShop.Identity.Application.Services;
using DevShop.Identity.Domain.Interfaces;
using DevShop.Identity.Infrastructure.Context;
using DevShop.Identity.Infrastructure.Repository;
using DevShop.WebAPI.Core.Identities;
using DevShop.WebAPI.Core.Middlewares;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DevShop.Identity.API.Configurations;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddScoped<INotify, Notify>();
        services.AddScoped<IAspNetUser, AspNetUser>();
        services.AddScoped<IAutenticationService, AutenticationService>();
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddAutoMapper(typeof(AutoMapperConfiguration).Assembly);
         
        services.AddValidatorsFromAssemblyContaining(typeof(CreateUserCommandValidation));
        services.AddMediatR(options => options.RegisterServicesFromAssemblies(new Assembly[] { typeof(LoginCommandValidator).Assembly,
            typeof(AutenticationService).Assembly,
            typeof(Entity).Assembly,
            typeof(AspNetUser).Assembly,
            typeof(Program).Assembly,
        }));

        services.AddUnhandledExceptionMiddlewareServices();
        services.AddValidationMiddlewareServices();
        services.AddValidationMiddlewareServices();

        services.AddRedisConfiguration(configuration);

        //services.AddRestClientConfiguration();
        services.AddIdentityConfiguration(configuration);

           

        //services.AddMiddlewareServices();
        services.AddHttpClientService()
            .AddDependencyInjectionRepository()
            .AddDependencyInjectionService()
            ;
        DependencyInjectionRepository(services);
 

        services.AddScoped<INotify, Notify>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();
        services.AddScoped<IDbInitializer, DbInitializer>();
        return services;
    }
 

    private static void DependencyInjectionRepository(IServiceCollection services)
    {

        services.AddScoped<ILogErrorRepository,LogErrorRepository>();
        services.AddScoped<ILogRequestRepository, LogRequestRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
          
    }
    private static IServiceCollection AddHttpClientService(this IServiceCollection services)
    {
        services.AddHttpClient();
        return services;
    }

    private static IServiceCollection AddDependencyInjectionService(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>();
        return services;
    }

    private static IServiceCollection AddDependencyInjectionRepository(this IServiceCollection services)
    {
        services.AddScoped<IRedisRepository, RedisRepository>();

        return services;
    }

    public static IServiceCollection AddUnhandledExceptionMiddlewareServices(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionMiddleware<,>));
        return services;
    }

    public static IServiceCollection AddValidationMiddlewareServices(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationMiddleware<,>));
        return services;
    }

    public static IServiceCollection AddAuthorizationBehaviorMiddlewareServices(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        return services;
    }


    public static IApplicationBuilder UseRequestResponseLoggingMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestResponseLoggingMiddleware>();
    }

    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}