namespace Trading.Application.BLL.Notifications;

public class TradingNotificationEvents : ITradingNotificationEvents
{
    public event EventHandler<TradingNotificationEventArgs>? TradingNotificationEvent;

    public void OnTradingNotification(TradingNotificationEventArgs e)
    {
        var handler = TradingNotificationEvent;
        handler?.Invoke(this, e);
    }
}
