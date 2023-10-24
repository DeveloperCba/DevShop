using DevShop.Core.Messages;
using DevShop.Identity.Application.Features.User.Dtos;
using MediatR;
namespace DevShop.Identity.Application.Features.Auth.Commands;

//public class LoginCommand : IRequest<UserResponseLoginDto>
//{
//    public string Login { get; set; }
//    public string Password { get; set; }
//}


public class LoginCommand : Command<UserResponseLoginDto>
{
    public string Login { get; set; }
    public string Password { get; set; }
}