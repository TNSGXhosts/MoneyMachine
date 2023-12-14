using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.TelegramConstants;
using Trading.Application.UserContext;

namespace Trading.Application.Handlers;

internal class SelectOrderHandler(IUserContext userContext) : IHandler
{
    public Triggers Trigger => Triggers.SelectOrder;

    public Tuple<string, InlineKeyboardMarkup> Handle(string userInput)
    {
        userContext.InputCallback = userInput;

        return new Tuple<string, InlineKeyboardMarkup>(
            "Choose an action:",
            new InlineKeyboardMarkup(new []
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Update Order", nameof(Triggers.EditOrder)),
                    InlineKeyboardButton.WithCallbackData("Close Order", nameof(Triggers.CloseOrder)),
                    InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)),
                }
            }));
    }
}