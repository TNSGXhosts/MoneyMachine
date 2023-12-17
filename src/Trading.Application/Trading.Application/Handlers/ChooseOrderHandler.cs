using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.BLL.CapitalIntegration;

using Trading.Application.TelegramConstants;
using Trading.Application.UserContext;

namespace Trading.Application.Handlers;

internal class ChooseOrderHandler(ICapitalClient capitalClient, IUserContext userContext) : IHandler
{
    public Triggers Trigger => Triggers.ChooseOrder;

    public Tuple<string, InlineKeyboardMarkup> Handle(string userInput)
    {
        var orders = capitalClient.GetOrders();
        var keyboardButtons = orders.Result
            .Select(
                o => InlineKeyboardButton.WithCallbackData(
                    o.MarketData.Epic,
                    $"{nameof(Triggers.SelectOrder)}-{o.WorkingOrderData.DealId}"))
            .ToList();
        keyboardButtons.Add(InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)));
        var keyboardMarkup = new InlineKeyboardMarkup(new []{ keyboardButtons });

        return new Tuple<string, InlineKeyboardMarkup>(
            "Choose an order:",
            keyboardMarkup);
    }
}