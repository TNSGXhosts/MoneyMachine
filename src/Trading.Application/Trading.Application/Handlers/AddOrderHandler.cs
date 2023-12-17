using Microsoft.Extensions.Logging;

using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.BLL.CapitalIntegration;

using Trading.Application.TelegramConstants;
using Trading.Application.UserContext;
using Trading.Application.UserInputPipeline;

namespace Trading.Application.Handlers;

internal class AddOrderHandler(IUserContext userContext, ICapitalClient capitalClient, ILogger<ParseTradeCreationStep> logger) : IHandler
{
    public Triggers Trigger => Triggers.AddOrder;

    public Tuple<string, InlineKeyboardMarkup> Handle(string userInput)
    {
        userContext.UserInputPipeline = new InputPipeline()
        {
            UserContext = userContext,
            PipelineSteps = new List<IPipelineStep>(){
                new ParseTradeCreationStep(userContext, logger, true),
                new CreateOrderStep(capitalClient, userContext),
            }
        };

        return new Tuple<string, InlineKeyboardMarkup>(
            "Enter Order Info: ticker, direction, size, level, stop loss, take profit",
             new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)),
                    }
                }));
    }
}