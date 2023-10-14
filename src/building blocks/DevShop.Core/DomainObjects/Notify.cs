namespace DevShop.Core.DomainObjects;

public class Notify : INotify
{
    private List<NotificationMessage> _notification;
    public Notify() => _notification = new List<NotificationMessage>();
    public void Handler(NotificationMessage notificacao) => _notification.Add(notificacao);
    public List<NotificationMessage> GetNotifications() => _notification;
    public bool HasNotification() => _notification.Any();
}

public class NotificationMessage
{
    public NotificationMessage(string message)
    {
        Message = message;
    }

    public string Message { get; }
}