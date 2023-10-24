using DevShop.Core.Messages;
using MediatR;

namespace DevShop.Identity.Application.Features.User.Events;

public class CreateUserEvent : Event 
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string CallBackUrl { get; set; }
    public Guid UserId { get; set; }

    public CreateUserEvent(string name, string email, string callBackUrl, Guid userId)
    {
        Name = name;
        Email = email;
        CallBackUrl = callBackUrl;
        UserId = userId;
        AggregateId = userId;
    }
}