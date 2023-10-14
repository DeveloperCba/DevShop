namespace DevShop.Core.DomainObjects;

public interface INotify
{
    bool HasNotification();
    List<NotificationMessage> GetNotifications();
    void Handler(NotificationMessage notifier);
}