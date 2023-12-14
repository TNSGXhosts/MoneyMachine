using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.BLL.CapitalIntegration;

using Trading.Application.TelegramConstants;
using Trading.Application.UserContext;
using Trading.Application.UserInputPipeline;

namespace Trading.Application.Handlers;

internal class AddPositionHandler(IUserContext userContext, ICapitalClient capitalClient) : IHandler
{
    public Triggers Trigger => Triggers.AddPosition;

    public Tuple<string, InlineKeyboardMarkup> Handle(string userInput)
    {
        userContext.UserInputPipeline = new InputPipeline()
        {
            UserContext = userContext,
            PipelineSteps = new List<IPipelineStep>(){
                new ParseTradeCreationStep(userContext),
                new CreatePositionStep(capitalClient, userContext),
            }
        };

        return new Tuple<string, InlineKeyboardMarkup>(
            "Enter Position Info: ticker, direction, size, stop loss, take profit",
             new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)),
                    }
                }));
    }
}