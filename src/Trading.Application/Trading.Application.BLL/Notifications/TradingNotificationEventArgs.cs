namespace Trading.Application.BLL.Notifications;

public class TradingNotificationEventArgs : EventArgs
{
    public TradingNotificationEventArgs(TradingNotification notification) => Notification = notification;

    public TradingNotification Notification { get; }
}
