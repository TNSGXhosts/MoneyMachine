using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.TelegramConstants;

namespace Trading.Application.Handlers;

internal class ChooseOrderHandler : IHandler
{
    public Triggers Trigger => Triggers.ChooseOrder;

    public Tuple<string, InlineKeyboardMarkup> Handle(string userInput)
    {
        //TODO: get orders from brocker
        var keyboardMarkup = new InlineKeyboardMarkup(new []
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("USDCHF", $"{nameof(Triggers.SelectOrder)}-123"),
                InlineKeyboardButton.WithCallbackData("EURUSD", $"{nameof(Triggers.SelectOrder)}-321"),
                InlineKeyboardButton.WithCallbackData("USDCAD", $"{nameof(Triggers.SelectOrder)}-142"),
                InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)),
            }
        });

        return new Tuple<string, InlineKeyboardMarkup>(
            "Choose an order:",
            keyboardMarkup);
    }
}