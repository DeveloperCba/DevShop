using Microsoft.Extensions.Configuration;

namespace DevShop.Core.Extensions;

public static class ConfigurationExtensions
{
    public static string GetMessageQueueConnection(this IConfiguration configuration, string name)
        => configuration?.GetSection("MessageQueueConnection")?[name];

    public static string GetProjectConnectionString(this IConfiguration configuration, string name)
        => configuration?.GetSection("ConnectionStrings")?[name];

}