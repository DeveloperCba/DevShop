using DevShop.Identity.Application.Features.User.Dtos;
using MediatR;
namespace DevShop.Identity.Application.Features.User.Commands;

public class SendTokenConfirmEmailCommand : IRequest<ConfirmEmailDto>
{
    public string UserId { get; set; }
    public string Email { get; set; }
}