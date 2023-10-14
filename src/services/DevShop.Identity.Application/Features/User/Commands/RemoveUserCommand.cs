using DevShop.Identity.Application.Models.Dtos;
using MediatR;
namespace DevShop.Identity.Application.Features.User.Commands;

public class RemoveUserCommand : IRequest<MessageDto>
{
    public string UserId { get; set; }
    public string Email { get; set; }
}