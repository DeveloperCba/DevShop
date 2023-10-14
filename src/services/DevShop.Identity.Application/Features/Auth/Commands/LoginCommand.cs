using DevShop.Identity.Application.Features.User.Dtos;
using MediatR;
namespace DevShop.Identity.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<UserResponseLoginDto>
{
    public string Login { get; set; }
    public string Password { get; set; }
}