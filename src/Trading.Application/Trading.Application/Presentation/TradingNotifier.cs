using System.Text;

using Microsoft.Extensions.Logging;

using Nito.AsyncEx;

using Trading.Application.BLL.Notifications;
using Trading.Application.BLL.Notifications.Types;
using Trading.Application.TelegramIntegration;

namespace Trading.Application.Presentation;

internal class TradingNotifier(ILogger<TradingNotifier> logger,
        ITradingNotificationEvents tradingNotificationEvents,
        ITelegramClient telegramClient)
    : ITradingNotifier
{
    private readonly Dictionary<TradingNotificationType, string> _tradingNotificationMap = new()
    {
        [TradingNotificationType.None] = "None",
        [TradingNotificationType.Buy] = "BUY",
        [TradingNotificationType.Sell] = "SELL"
    };

    private readonly Dictionary<TradingNotificationDealType, string> _tradingNotificationDealMap = new()
    {
        [TradingNotificationDealType.None] = "None",
        [TradingNotificationDealType.Long] = "LONG",
        [TradingNotificationDealType.Short] = "SHORT"
    };

    public void Subscribe() => tradingNotificationEvents.TradingNotificationEvent += OnTradingNotificationEvent!;

#pragma warning disable RCS1163
    private void OnTradingNotificationEvent(object sender, TradingNotificationEventArgs e)
#pragma warning restore RCS1163
    {
        var message = BuildMessage(e);

        logger.LogInformation("Trading notification event received: {0}", message);
        AsyncContext.Run(() => telegramClient.SendMessageAsync(message));
    }

    // TODO : Build message in a better way.
    private string BuildMessage(TradingNotificationEventArgs e)
    {
        var message = new StringBuilder();
        message.Append("Trading notification event received: ")
            .AppendLine(_tradingNotificationMap.First(x => x.Key == e.Notification.Type).Value);
        message.Append("Deal type: ")
            .AppendLine(_tradingNotificationDealMap.First(x => x.Key == e.Notification.DealType).Value);

        return message.ToString();
    }
}
