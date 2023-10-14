namespace DevShop.Core.Comunications;

public class RedisSettings
{
    public int AbsoluteExpirationInMinutes { get; set; }
    public int SlidingExpirationInMinutes { get; set; }
    public string Host { get; set; }
    public string Port { get; set; }
    public string ChannelPrefix { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
}