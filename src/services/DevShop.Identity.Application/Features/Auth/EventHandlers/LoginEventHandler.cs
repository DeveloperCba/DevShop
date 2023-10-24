using DevShop.Identity.Application.Features.User.Events;
using MediatR;

namespace DevShop.Identity.Application.Features.Auth.EventHandlers;

public class LoginEventHandler : INotificationHandler<LoginEvent>
{
    public Task Handle(LoginEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}