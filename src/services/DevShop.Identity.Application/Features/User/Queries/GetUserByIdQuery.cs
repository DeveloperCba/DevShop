using DevShop.Identity.Application.Features.User.Dtos;
using MediatR;
namespace DevShop.Identity.Application.Features.User.Queries;

public class GetUserByIdQuery : IRequest<UserDto>
{
    public string UserId { get; set; }
}