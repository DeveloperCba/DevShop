using DevShop.Core.Comunications;
using DevShop.Core.Datas.Implementations;
using DevShop.Core.Datas.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Core.Configurations.Redis;

public static class RedisConfiguration
{
    public static IServiceCollection AddRedisConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDistributedCache, RedisCache>();
        services.AddScoped(typeof(IRedisRepository), typeof(RedisRepository));

        var redisSettingsSection = configuration.GetSection(nameof(RedisSettings));
        services.Configure<RedisSettings>(redisSettingsSection);
        var cacheSettings = redisSettingsSection.Get<RedisSettings>();
        services.AddStackExchangeRedisCache(options =>
        {
            options.ConfigurationOptions = GetOptionsCache(cacheSettings);
            options.InstanceName = cacheSettings.ChannelPrefix;
        });

        return services;
    }

    public static IServiceCollection AddRedisConfiguration(this IServiceCollection services, RedisSettings cacheSettings)
    {
        services.AddSingleton<IDistributedCache, RedisCache>();
        services.AddScoped(typeof(IRedisRepository), typeof(RedisRepository));
        services.AddStackExchangeRedisCache(options =>
        {
            options.ConfigurationOptions = GetOptionsCache(cacheSettings);
            options.InstanceName = cacheSettings.ChannelPrefix;

        });

        return services;
    }

    private static ConfigurationOptions GetOptionsCache(RedisSettings cacheSettings = null)
    {
        var serverPort = string.IsNullOrEmpty(cacheSettings.Port) ? string.Empty : ":" + cacheSettings.Port;
        var serverHost = $"{cacheSettings.Host}{serverPort}";
        var configurationOptions = ConfigurationOptions.Parse(serverHost);
        configurationOptions.User = cacheSettings.User;
        configurationOptions.Password = cacheSettings.Password;
        configurationOptions.ChannelPrefix = new RedisChannel(cacheSettings?.ChannelPrefix,RedisChannel.PatternMode.Auto);
        configurationOptions.ClientName = cacheSettings.ChannelPrefix;
        configurationOptions.AbortOnConnectFail = false;
        configurationOptions.ReconnectRetryPolicy = new ExponentialRetry(5000);
        return configurationOptions;
    }
}