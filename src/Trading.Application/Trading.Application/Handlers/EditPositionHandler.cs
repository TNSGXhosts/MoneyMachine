using Microsoft.Extensions.Logging;

using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.BLL.CapitalIntegration;

using Trading.Application.TelegramConstants;
using Trading.Application.UserContext;
using Trading.Application.UserInputPipeline;

namespace Trading.Application.Handlers;

internal class EditPositionHandler(
    IPositionClient positionClient,
    IUserContext userContext,
    ILogger<ParseTradeUpdateStep> logger) : IHandler
{
    public Triggers Trigger => Triggers.EditPosition;

    public Tuple<string, InlineKeyboardMarkup> Handle(string userInput)
    {
        userContext.UserInputPipeline = new InputPipeline()
        {
            UserContext = userContext,
            PipelineSteps = new List<IPipelineStep>(){
                new ParseTradeUpdateStep(userContext, logger, false),
                new UpdatePositionStep(positionClient, userContext),
            }
        };

        return new Tuple<string, InlineKeyboardMarkup>(
            GetMessage(),
            new InlineKeyboardMarkup(new []
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)),
                }
            }));
    }

    private string GetMessage()
    {
        var message = "Enter new Position info: Take profit, Stop loss";
        message = string.Concat(message, Environment.NewLine, $"`{userContext.PositionData.Position.StopLevel}");
        message = string.Concat(message, Environment.NewLine, $"{userContext.PositionData.Position.ProfitLevel}`");

        return message;
    }
}