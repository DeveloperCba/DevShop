using DevShop.Identity.Application.Features.User.Dtos;
using MediatR;
namespace DevShop.Identity.Application.Features.User.Queries;

public class GetUserByUserNameQuery : IRequest<UserDto>
{
    public string UserName { get; set; }
}