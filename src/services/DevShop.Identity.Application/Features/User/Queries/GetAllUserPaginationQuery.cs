using DevShop.Identity.Application.Features.User.Dtos;
using MediatR;
namespace DevShop.Identity.Application.Features.User.Queries;

public class GetAllUserPaginationQuery : IRequest<List<UserDto>>
{
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 0;
    public string Filter { get; set; } = string.Empty;
}