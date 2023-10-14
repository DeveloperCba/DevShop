namespace DevShop.Identity.Application.Features.User.Dtos;

public class UserResponseLoginDto
{
    public string AccessToken { get; set; }
    public Guid RefreshToken { get; set; }
    public double ExpiresIn { get; set; }
    public UserTokenDto UsuarioToken { get; set; }
}