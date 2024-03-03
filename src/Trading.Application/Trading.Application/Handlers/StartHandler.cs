using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.TelegramConstants;

namespace Trading.Application.Handlers;

internal class StartHandler : IHandler
{
    public Triggers Trigger => Triggers.Start;

    public Tuple<string, InlineKeyboardMarkup> Handle(string userInput)
    {
        if (!userInput.Equals(nameof(Trigger.Start)))
        {
            return new Tuple<string, InlineKeyboardMarkup>(string.Empty, null);
        }

        return new Tuple<string, InlineKeyboardMarkup>(
            "Choose an action:",
            new InlineKeyboardMarkup(new []
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Add Order:", nameof(Triggers.AddOrder)),
                    InlineKeyboardButton.WithCallbackData("Add Position:", nameof(Triggers.AddPosition)),
                    InlineKeyboardButton.WithCallbackData("Choose Order:", nameof(Triggers.ChooseOrder)),
                    InlineKeyboardButton.WithCallbackData("Choose Position:", nameof(Triggers.ChoosePosition)),
                    InlineKeyboardButton.WithCallbackData("Test Strategy:", nameof(Triggers.TestStrategy))
                }
            })
        );
    }
}