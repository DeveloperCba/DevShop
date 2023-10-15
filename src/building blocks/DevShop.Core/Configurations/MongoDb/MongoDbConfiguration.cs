using DevShop.Core.Comunications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Core.Configurations.MongoDb;

public static class MongoDbConfiguration
{
    public static IServiceCollection AddMongoDbConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbSettingsSection = configuration.GetSection(nameof(MongoDbSettings));
        services.Configure<MongoDbSettings>(mongoDbSettingsSection);
        var cacheSettings = mongoDbSettingsSection.Get<MongoDbSettings>();
        services.AddSingleton<IMongoClient, MongoClient>(option =>
        {
            return new MongoClient(cacheSettings?.ConnectionStrings);
        });
        return services;
    }
}