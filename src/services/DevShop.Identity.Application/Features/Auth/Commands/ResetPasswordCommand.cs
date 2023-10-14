using DevShop.Identity.Application.Features.Auth.Dtos;
using MediatR;
namespace DevShop.Identity.Application.Features.Auth.Commands;

public class ResetPasswordCommand : IRequest<ResetPasswordDto>
{
    public string Email { get; set; }
}