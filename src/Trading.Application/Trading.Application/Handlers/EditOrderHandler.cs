using Microsoft.Extensions.Logging;

using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.BLL.CapitalIntegration;

using Trading.Application.TelegramConstants;
using Trading.Application.TelegramIntegration;
using Trading.Application.UserContext;
using Trading.Application.UserInputPipeline;

namespace Trading.Application.Handlers;

internal class EditOrderHandler(ICapitalClient capitalClient, IUserContext userContext, ILogger<ParseTradeUpdateStep> logger) : IHandler
{
    public Triggers Trigger => Triggers.EditOrder;

    public Tuple<string, InlineKeyboardMarkup> Handle(string userInput)
    {
        userContext.UserInputPipeline = new InputPipeline()
        {
            UserContext = userContext,
            PipelineSteps = new List<IPipelineStep>(){
                new ParseTradeUpdateStep(userContext, logger, true),
                new UpdateOrderStep(capitalClient, userContext),
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
        var message = "Enter new Order info:";
        message = string.Concat(message, Environment.NewLine, "Order Level, Stop Distance, Profit Distance");
        message = string.Concat(message, Environment.NewLine, $"`{userContext.WorkingOrder.WorkingOrderData.OrderLevel}");
        message = string.Concat(message, Environment.NewLine, $"{userContext.WorkingOrder.WorkingOrderData.StopDistance}");
        message = string.Concat(message, Environment.NewLine, $"{userContext.WorkingOrder.WorkingOrderData.ProfitDistance}`");

        return message;
    }
}