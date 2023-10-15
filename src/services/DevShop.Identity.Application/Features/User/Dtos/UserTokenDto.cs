namespace DevShop.Identity.Application.Features.User.Dtos;

public class UserTokenDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public IEnumerable<UserClaimDto> Claims { get; set; }
}