namespace DevShop.Core.Comunications;

public class JwtSettings
{
    public string AuthenticationJwksUrl { get; set; }
    public int? TokenExpiration { get; set; }
    public int? RefreshTokenExpiration { get; set; }
    public string ValidOn { get; set; }
    public string Issuer { get; set; }
    public string Secret { get; set; }
}