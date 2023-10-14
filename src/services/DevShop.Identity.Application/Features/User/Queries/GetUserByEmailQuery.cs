using DevShop.Identity.Application.Features.User.Dtos;
using MediatR;
namespace DevShop.Identity.Application.Features.User.Queries;

public class GetUserByEmailQuery : IRequest<UserDto>
{
    public string Email { get; set; }
}