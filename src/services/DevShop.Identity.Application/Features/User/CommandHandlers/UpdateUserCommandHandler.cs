using DevShop.Core.DomainObjects;
using DevShop.Identity.Application.Features.User.Commands;
using DevShop.Identity.Application.Models.Dtos;
using DevShop.Identity.Application.Services;
using DevShop.Identity.Domain.Models;
using DevShop.WebAPI.Core.Services;
using MediatR;
namespace DevShop.Identity.Application.Features.User.CommandHandlers;

public class UpdateUserCommandHandler : BaseService, IRequestHandler<UpdateUserCommand, MessageDto>
{
    private readonly IAutenticationService _autenticationService;

    public UpdateUserCommandHandler(
        INotify notification,
        IAutenticationService autenticationService) : base(notification)
    {
        _autenticationService = autenticationService;
    }

    public async Task<MessageDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _autenticationService.UserManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            NotificationEvent("Usuário não encontrado.");
            return default!;
        }

        user = GetUserMap(request, user);
        var result = await _autenticationService.UserManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return new MessageDto
            {
                Message = "Usuário alterado com sucesso!!!"
            };
        }

        foreach (var error in result.Errors)
            NotificationEvent(error.Description);

        return default!;
    }
    private static ApplicationUser GetUserMap(UpdateUserCommand request, ApplicationUser user)
    {
        user.Neighborhood = request.Neighborhood;
        user.ZipCode = request.ZipCode;
        user.City = request.City;
        user.Complement = request.Complement;
        user.Document = request.Document;
        user.Email = request.Email;
        user.UserName = request.Email;
        user.Number = request.Number;
        user.State = request.State;
        user.Street = request.Street;
        user.Name = request.Name;
        user.PhoneNumber = request.PhoneNumber;
        user.EmailConfirmed = true;
        user.TwoFactorEnabled = true;

        return user;
    }
}