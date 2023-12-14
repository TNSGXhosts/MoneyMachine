using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.TelegramConstants;

namespace Trading.Application.Handlers;

internal class ChoosePositionHandler : IHandler
{
    public Triggers Trigger => Triggers.ChoosePosition;

    public Tuple<string, InlineKeyboardMarkup> Handle(string userInput)
    {
        //TODO: get positions from brocker
        var keyboardMarkup = new InlineKeyboardMarkup(new []
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("USDCHF", $"{nameof(Triggers.SelectPosition)}-123"),
                InlineKeyboardButton.WithCallbackData("EURUSD", $"{nameof(Triggers.SelectPosition)}-321"),
                InlineKeyboardButton.WithCallbackData("USDCAD", $"{nameof(Triggers.SelectPosition)}-142"),
                InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)),
            }
        });

        return new Tuple<string, InlineKeyboardMarkup>(
            "Choose a position:",
            keyboardMarkup);
    }
}