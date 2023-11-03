using Microsoft.Extensions.Logging;

using Trading.Application.BLL.Notifications;
using Trading.Application.BLL.Notifications.Types;

namespace Trading.Application.BLL.TradingHandler;

internal class CapitalTradingHandlerProcessor : ICapitalTradingHandlerProcessor
{
    private readonly ILogger<CapitalTradingHandlerProcessor> _logger;
    private readonly ITradingNotificationEvents _tradingNotificationEvents;

    public CapitalTradingHandlerProcessor(ILogger<CapitalTradingHandlerProcessor> logger,
        ITradingNotificationEvents tradingNotificationEvents)
    {
        _logger = logger;
        _tradingNotificationEvents = tradingNotificationEvents;
    }

    public Task ProcessAsync()
    {
        // TODO : Implement
        var random = new Random();
        var tradingNotificationType = typeof(TradingNotificationType);
        var notificationTypeValues = tradingNotificationType.GetEnumValues();
        var tradingNotificationDealType = typeof(TradingNotificationDealType);
        var notificationTypeDealValues = tradingNotificationDealType.GetEnumValues();

        var indexType = random.Next(notificationTypeValues.Length);
        var indexDealType = random.Next(notificationTypeDealValues.Length);

        var notification = new TradingNotification
        {
            Type = (TradingNotificationType)notificationTypeValues.GetValue(indexType)!,
            DealType = (TradingNotificationDealType)notificationTypeDealValues.GetValue(indexDealType)!
        };
        var eventArgs = new TradingNotificationEventArgs(notification);
        _tradingNotificationEvents.OnTradingNotification(eventArgs);

        return Task.CompletedTask;
    }
}
