using DevShop.Core.Messages;

namespace DevShop.Identity.Application.Features.User.Events;

public class LoginEvent : Event
{
    public string Name { get; set; }
    public string Email { get; set; }
    public Guid UserId { get; set; }

    public LoginEvent(string name, string email, Guid userId)
    {
        Name = name;
        Email = email;
        UserId = userId;
        AggregateId = userId;
    }
}