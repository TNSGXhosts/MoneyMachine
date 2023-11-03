using System.Text;

using Microsoft.Extensions.Logging;

using Nito.AsyncEx;

using Trading.Application.BLL.Notifications;
using Trading.Application.BLL.Notifications.Types;
using Trading.Application.TelegramIntegration;

namespace Trading.Application.Presentation;

internal class TradingNotifier : ITradingNotifier
{
    private readonly ILogger<TradingNotifier> _logger;
    private readonly ITradingNotificationEvents _tradingNotificationEvents;
    private readonly ITelegramClient _telegramClient;

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

    public TradingNotifier(ILogger<TradingNotifier> logger,
        ITradingNotificationEvents tradingNotificationEvents,
        ITelegramClient telegramClient)
    {
        _logger = logger;
        _tradingNotificationEvents = tradingNotificationEvents;
        _telegramClient = telegramClient;
    }

    public void Subscribe() => _tradingNotificationEvents.TradingNotificationEvent += OnTradingNotificationEvent!;

    private void OnTradingNotificationEvent(object sender, TradingNotificationEventArgs e)
    {
        var message = BuildMessage(e);

        _logger.LogInformation("Trading notification event received: {0}", message);
        AsyncContext.Run(() => _telegramClient.SendMessageAsync(message));
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
