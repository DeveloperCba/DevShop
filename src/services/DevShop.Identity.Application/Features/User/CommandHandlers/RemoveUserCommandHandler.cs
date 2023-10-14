using DevShop.Core.DomainObjects;
using DevShop.Identity.Application.Features.User.Commands;
using DevShop.Identity.Application.Models.Dtos;
using DevShop.Identity.Application.Services;
using DevShop.WebAPI.Core.Identities;
using DevShop.WebAPI.Core.Services;
using MediatR;
namespace DevShop.Identity.Application.Features.User.CommandHandlers;

public class RemoveUserCommandHandler : BaseService, IRequestHandler<RemoveUserCommand, MessageDto>
{
    private readonly IAutenticationService _autenticationService;
    private readonly IAspNetUser _aspNetUser;

    public RemoveUserCommandHandler(
        INotify notification,
        IAutenticationService autenticationService,
        IAspNetUser aspNetUser) : base(notification)
    {
        _autenticationService = autenticationService;
        _aspNetUser = aspNetUser;
    }

    public async Task<MessageDto> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _autenticationService.UserManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            NotificationEvent("Usuário não encontrado.");
            return default!;
        }

        if (request.UserId == _aspNetUser.GetUserId().ToString())
        {
            NotificationEvent($"O {user.Name} não pode ser excluído por si mesmo, tente logar com outro usuário e tente novamente.");
            return default!;
        }

        var result = await _autenticationService.UserManager.DeleteAsync(user);
        if (result.Succeeded)
            return new MessageDto { Message = "Usuário excluído com sucesso!!!" };

        foreach (var error in result.Errors)
            NotificationEvent(error.Description);

        return default!;
    }
}