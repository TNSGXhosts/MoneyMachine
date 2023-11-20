namespace Trading.Application.BLL.Notifications;

public class TradingNotificationEventArgs(TradingNotification notification) : EventArgs
{
    public TradingNotification Notification { get; } = notification;
}
