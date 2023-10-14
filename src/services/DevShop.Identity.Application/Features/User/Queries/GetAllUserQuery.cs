using DevShop.Identity.Application.Features.User.Dtos;
using MediatR;
namespace DevShop.Identity.Application.Features.User.Queries;

public class GetAllUserQuery : IRequest<List<UserDto>>
{
    public string Filter { get; set; }
}