using DevShop.Identity.Application.Models.Dtos;
using MediatR;
namespace DevShop.Identity.Application.Features.User.Commands;

public class LockUserCommand : IRequest<MessageDto>
{
    public string UserId { get; set; }
}