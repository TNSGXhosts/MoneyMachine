using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.BLL.CapitalIntegration;

using Trading.Application.TelegramConstants;
using Trading.Application.UserContext;

namespace Trading.Application.Handlers;

internal class ChooseOrderHandler(IOrderClient orderClient, IUserContext userContext) : IHandler
{
    public Triggers Trigger => Triggers.ChooseOrder;

    public Tuple<string, InlineKeyboardMarkup> Handle(string userInput)
    {
        var orders = orderClient.GetOrdersAsync();
        var keyboardButtons = orders.Result
            .Select(
                o => InlineKeyboardButton.WithCallbackData(
                    $"{o.MarketData.Epic}-{o.WorkingOrderData.OrderLevel}",
                    $"{nameof(Triggers.SelectOrder)}{ParserConstants.ParserConstants.Separator}{o.WorkingOrderData.DealId}"
                    ))
            .ToList();

        var buttonLines = new List<List<InlineKeyboardButton>>();
        for (var i = 0; i < keyboardButtons.Count; i += 4)
        {
            var subArray = keyboardButtons.GetRange(i, Math.Min(4, keyboardButtons.Count - i));
            buttonLines.Add(subArray);
        }

        buttonLines.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)) });
        var keyboardMarkup = new InlineKeyboardMarkup(buttonLines);

        return new Tuple<string, InlineKeyboardMarkup>(
            "Choose an order:",
            keyboardMarkup);
    }
}