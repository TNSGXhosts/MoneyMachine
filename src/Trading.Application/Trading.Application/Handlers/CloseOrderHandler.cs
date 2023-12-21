using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.BLL.CapitalIntegration;
using Trading.Application.TelegramConstants;
using Trading.Application.UserContext;

namespace Trading.Application.Handlers;

public class CloseOrderHandler(ICapitalClient capitalClient, IUserContext userContext) : IHandler
{
    Triggers IHandler.Trigger => Triggers.CloseOrder;

    public Tuple<string, InlineKeyboardMarkup> Handle(string userInput)
    {
        var isSuccess = capitalClient.CloseOrderAsync(userContext.InputCallback).Result;
        userContext.InputCallback = string.Empty;

        return new Tuple<string, InlineKeyboardMarkup>(
            isSuccess ? "Operation have processed successfully" : "Operation Failed",
            new InlineKeyboardMarkup(new [] { new [] { InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)) } }));
    }
}