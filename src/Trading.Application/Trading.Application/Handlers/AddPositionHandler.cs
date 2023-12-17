using Microsoft.Extensions.Logging;

using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.BLL.CapitalIntegration;

using Trading.Application.TelegramConstants;
using Trading.Application.UserContext;
using Trading.Application.UserInputPipeline;

namespace Trading.Application.Handlers;

internal class AddPositionHandler(IUserContext userContext, ICapitalClient capitalClient, ILogger<ParseTradeCreationStep> logger) : IHandler
{
    public Triggers Trigger => Triggers.AddPosition;

    public Tuple<string, InlineKeyboardMarkup> Handle(string userInput)
    {
        userContext.UserInputPipeline = new InputPipeline()
        {
            UserContext = userContext,
            PipelineSteps = new List<IPipelineStep>(){
                new ParseTradeCreationStep(userContext, logger, false),
                new CreatePositionStep(capitalClient, userContext),
            }
        };

        return new Tuple<string, InlineKeyboardMarkup>(
            @"Enter Position Info:
            Ticker - 'SILVER'
            Direction - BUY/SELL
            Size - 1
            Stop loss - optional
            Take profit - optional",
             new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)),
                    }
                }));
    }
}