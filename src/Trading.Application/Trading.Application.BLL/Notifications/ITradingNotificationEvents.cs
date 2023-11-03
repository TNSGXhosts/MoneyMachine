namespace Trading.Application.BLL.Notifications;

public interface ITradingNotificationEvents
{
    event EventHandler<TradingNotificationEventArgs> TradingNotificationEvent;

    void OnTradingNotification(TradingNotificationEventArgs e);
}
