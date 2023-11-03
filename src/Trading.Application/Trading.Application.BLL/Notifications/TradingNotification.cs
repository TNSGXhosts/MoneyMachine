using Trading.Application.BLL.Notifications.Types;

namespace Trading.Application.BLL.Notifications;

public class TradingNotification
{
    public required TradingNotificationType Type { get; init; }

    public required TradingNotificationDealType DealType { get; init; }
}
