using Microsoft.Extensions.Logging;

using Trading.Application.BLL.Notifications;
using Trading.Application.BLL.Notifications.Types;

namespace Trading.Application.BLL.TradingHandler;

internal class CapitalTradingHandlerProcessor(ILogger<CapitalTradingHandlerProcessor> logger,
        ITradingNotificationEvents tradingNotificationEvents)
    : ICapitalTradingHandlerProcessor
{
#pragma warning disable CA1823
    // ReSharper disable once UnusedMember.Local
#pragma warning disable RCS1213
    private readonly ILogger<CapitalTradingHandlerProcessor> _logger = logger;
#pragma warning restore RCS1213
#pragma warning restore CA1823

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
        tradingNotificationEvents.OnTradingNotification(eventArgs);

        return Task.CompletedTask;
    }
}
