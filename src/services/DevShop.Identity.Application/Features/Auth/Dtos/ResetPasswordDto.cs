namespace DevShop.Identity.Application.Features.Auth.Dtos;

public class ResetPasswordDto
{
    public string UserId { get; set; }
    public string Token { get; set; }
    public string CallBack { get; set; }
}