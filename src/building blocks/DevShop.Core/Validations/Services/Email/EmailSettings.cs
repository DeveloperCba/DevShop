namespace DevShop.Core.Validations.Services.Email;

public class EmailSettings
{
    public string Host { get; set; }
    public string Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string SenderAddress { get; set; }
    public bool EnableSsl { get; set; }
}