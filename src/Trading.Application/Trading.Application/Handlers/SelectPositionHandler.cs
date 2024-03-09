using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.BLL.CapitalIntegration;

using Trading.Application.TelegramConstants;
using Trading.Application.UserContext;

namespace Trading.Application.Handlers;

internal class SelectPositionHandler(IUserContext userContext, IPositionClient positionClient) : IHandler
{
    public Triggers Trigger => Triggers.SelectPosition;

    public Task<Tuple<string, InlineKeyboardMarkup>> HandleAsync(string userInput)
    {
        userContext.InputCallback = userInput;

        userContext.PositionData = positionClient.GetPositionsAsync().Result.First(p => p.Position.DealId == userInput);

        return Task.FromResult(new Tuple<string, InlineKeyboardMarkup>(
            "Choose an action:",
            new InlineKeyboardMarkup(new []
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Update Position", nameof(Triggers.EditPosition)),
                    InlineKeyboardButton.WithCallbackData("Close Position", nameof(Triggers.ClosePosition)),
                    InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)),
                }
            })));
    }
}