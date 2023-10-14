using DevShop.Core.DomainObjects;
using DevShop.Identity.Application.Features.User.Commands;
using DevShop.Identity.Application.Models.Dtos;
using DevShop.Identity.Application.Services;
using DevShop.WebAPI.Core.Identities;
using DevShop.WebAPI.Core.Services;
using MediatR;
namespace DevShop.Identity.Application.Features.User.CommandHandlers;

public class LockUnlockCommandHandler : BaseService,
    IRequestHandler<LockUserCommand, MessageDto>,
    IRequestHandler<UnlockUserCommand, MessageDto>
{
    private readonly IAutenticationService _autenticationService;
    private readonly IAspNetUser _aspNetUser;
    public LockUnlockCommandHandler(
        INotify notification,
        IAutenticationService autenticationService,
        IAspNetUser aspNetUser) : base(notification)
    {
        _autenticationService = autenticationService;
        _aspNetUser = aspNetUser;
    }

    public async Task<MessageDto> Handle(LockUserCommand request, CancellationToken cancellationToken)
    {
        var response = await LockAndUnlockUser(userId: request.UserId, statusLock: true);
        return response;
    }

    public async Task<MessageDto> Handle(UnlockUserCommand request, CancellationToken cancellationToken)
    {
        var response = await LockAndUnlockUser(userId: request.UserId, statusLock: false);
        return response;
    }

    private async Task<MessageDto> LockAndUnlockUser(string userId, bool statusLock)
    {
        var user = await _autenticationService.UserManager.FindByIdAsync(userId);
        var message = new MessageDto();
        if (user == null)
        {
            NotificationEvent("Usuário não encontrado.");
            return default!;
        }

        if (_aspNetUser.UserInformedEhLogged(userId))
        {
            NotificationEvent($"O {user.Name} não pode ser {(statusLock ? "bloqueado" : "desbloqueado")} por si mesmo, tente logar com outro usuário e tente novamente.");
            return default!;
        }

        if (statusLock)
        {
            user.LockoutEnd = DateTime.Now.AddYears(1000);
            message.Message = "Usuário bloqueado com sucesso!";
        }
        else
        {
            user.LockoutEnd = DateTime.Now;
            message.Message = "Usuário desbloqueado com sucesso!";
        }
        await _autenticationService.UserManager.UpdateAsync(user);

        return message;
    }
}