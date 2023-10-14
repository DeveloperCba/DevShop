using DevShop.Core.Datas.Enumerators;
using DevShop.Identity.Domain.Models;
using DevShop.Identity.Infrastructure.Context;
using DevShop.WebAPI.Core.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Identity.API.Configurations;

public static class IdentityConfiguration
{
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString("PostgresConnection");
        AddDbAplicationUserIdentity(services, conn);
        AddDbLog(services, conn);

        services.AddDefaultIdentity<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
            .AddErrorDescriber<IdentityMessagePortugues>()
            .AddDefaultTokenProviders();

        // Identity Options
        services.AddIdentityOptions();
        // JWT
        services.AddJwtConfiguration(configuration);
        return services;
    }

    private static IServiceCollection AddIdentityOptions(this IServiceCollection services, IdentityOptions identityOptions = null)
    {
        if (identityOptions != null)
        {
            return services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = identityOptions.SignIn.RequireConfirmedEmail;
                options.Lockout.DefaultLockoutTimeSpan = identityOptions.Lockout.DefaultLockoutTimeSpan;
                options.Lockout.MaxFailedAccessAttempts = identityOptions.Lockout.MaxFailedAccessAttempts;
                options.User.RequireUniqueEmail = identityOptions.User.RequireUniqueEmail;
                options.Password.RequiredLength = identityOptions.Password.RequiredLength;
                options.Password.RequireDigit = identityOptions.Password.RequireDigit;
                options.Password.RequireLowercase = identityOptions.Password.RequireLowercase;
                options.Password.RequireUppercase = identityOptions.Password.RequireUppercase;
                options.Password.RequireNonAlphanumeric = identityOptions.Password.RequireNonAlphanumeric;
                options.Password.RequiredUniqueChars = identityOptions.Password.RequiredUniqueChars;
            });
        }
        return services.Configure<IdentityOptions>(options =>
        {
            //options.SignIn.RequireConfirmedEmail = false;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            options.Lockout.MaxFailedAccessAttempts = 3;
            //options.User.RequireUniqueEmail = true;
            //options.Password.RequiredLength = 8;
            //options.Password.RequireDigit = false;
            //options.Password.RequireLowercase = false;
            //options.Password.RequireUppercase = false;
            //options.Password.RequireNonAlphanumeric = false;
            //options.Password.RequiredUniqueChars = 6;
        });
    }

    private static void AddDbAplicationUserIdentity(IServiceCollection services, string? conn)
    {
        var migrationName = "__migrationTable";
        services.AddDbContext<ApplicationIdentityDbContext>(options =>
            options.UseNpgsql(conn, x => x.MigrationsHistoryTable(migrationName)));
    }


    private static void AddDbLog(IServiceCollection services, string? conn)
    {
        var migrationName = "__migrationLog";
        services.AddDbContext<LogDbContext>(options =>
            options.UseNpgsql(conn, x => x.MigrationsHistoryTable(migrationName)));
    }
}