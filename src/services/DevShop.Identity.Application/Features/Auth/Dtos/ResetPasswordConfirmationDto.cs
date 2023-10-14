namespace DevShop.Identity.Application.Features.Auth.Dtos;

public class ResetPasswordConfirmationDto
{
    public string UserId { get; set; }
    public string Token { get; set; }
    public string Password { get; set; }
}