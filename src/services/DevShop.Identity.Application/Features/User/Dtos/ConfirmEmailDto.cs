namespace DevShop.Identity.Application.Features.User.Dtos;

public class ConfirmEmailDto
{
    public string UserId { get; set; }
    public string Token { get; set; }
    public string CallBack { get; set; }
}