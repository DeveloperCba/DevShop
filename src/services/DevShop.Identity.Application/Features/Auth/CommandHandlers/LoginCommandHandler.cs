using DevShop.Core.DomainObjects;
using DevShop.Identity.Application.Contracts;
using DevShop.Identity.Application.Features.Auth.Commands;
using DevShop.Identity.Application.Features.User.Dtos;
using DevShop.Identity.Domain.Interfaces;
using DevShop.WebAPI.Core.Services;
using MediatR;
namespace DevShop.Identity.Application.Features.Auth.CommandHandlers;

public class LoginCommandHandler : BaseService, IRequestHandler<LoginCommand, UserResponseLoginDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IAutenticationService _autenticationService;

    public LoginCommandHandler(
        IUserRepository userRepository,
        INotify notification,
        IAutenticationService autenticationService
        ) : base(notification)
    {
        _userRepository = userRepository;
        _autenticationService = autenticationService;
    }

    public async Task<UserResponseLoginDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByFilterAsync(x => x.Document == request.Login || x.Email == request.Login);
        if (user is null)
        {
            NotificationEvent("Usuário ou senha inválida!!!");
            return default!;
        }
        var result = await _autenticationService.SignInManager.PasswordSignInAsync(user.Email, request.Password, false, true);

        if (result.RequiresTwoFactor)
        {
            NotificationEvent("Usuário temporariamente bloqueado por tentativas inválidas.");
            return default!;
        }

        if (result.IsNotAllowed)
        {
            NotificationEvent("Usuário precisa confirmar o email de cadastro.");
            return default!;
        }

        if (result.Succeeded)
        {
            return await _autenticationService.GenerateJwt(user.Email);
        }

        if (result.IsLockedOut)
        {
            NotificationEvent("Usuário temporariamente bloqueado por tentativas inválidas.");
            return default!;
        }

        NotificationEvent("Usuário ou senha incorreto.");
        return default!;

    }
}