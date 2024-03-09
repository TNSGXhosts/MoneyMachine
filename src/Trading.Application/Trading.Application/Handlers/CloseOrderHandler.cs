using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.BLL.CapitalIntegration;
using Trading.Application.TelegramConstants;
using Trading.Application.UserContext;

namespace Trading.Application.Handlers;

public class CloseOrderHandler(IOrderClient orderClient, IUserContext userContext) : IHandler
{
    Triggers IHandler.Trigger => Triggers.CloseOrder;

    public Task<Tuple<string, InlineKeyboardMarkup>> HandleAsync(string userInput)
    {
        var isSuccess = orderClient.CloseOrderAsync(userContext.InputCallback).Result;
        userContext.InputCallback = string.Empty;

        return Task.FromResult(new Tuple<string, InlineKeyboardMarkup>(
            isSuccess ? "Operation have processed successfully" : "Operation Failed",
            new InlineKeyboardMarkup(new [] { new [] { InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)) } })));
    }
}