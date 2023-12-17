using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.BLL.CapitalIntegration;

using Trading.Application.TelegramConstants;

namespace Trading.Application.Handlers;

internal class ChoosePositionHandler(ICapitalClient capitalClient) : IHandler
{
    public Triggers Trigger => Triggers.ChoosePosition;

    public Tuple<string, InlineKeyboardMarkup> Handle(string userInput)
    {
        var positions = capitalClient.GetPositions();
        var keyboardButtons = positions.Result
            .Select(
                o => InlineKeyboardButton.WithCallbackData(o.Market.Epic, $"{nameof(Triggers.SelectOrder)}-{o.Position.DealId}"))
            .ToList();
        keyboardButtons.Add(InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)));

        var keyboardMarkup = new InlineKeyboardMarkup(new [] { keyboardButtons });

        return new Tuple<string, InlineKeyboardMarkup>(
            "Choose a position:",
            keyboardMarkup);
    }
}