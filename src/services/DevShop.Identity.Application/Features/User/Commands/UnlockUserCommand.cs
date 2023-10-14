using DevShop.Identity.Application.Models.Dtos;
using MediatR;
namespace DevShop.Identity.Application.Features.User.Commands;

public class UnlockUserCommand : IRequest<MessageDto>
{
    public string UserId { get; set; }
}