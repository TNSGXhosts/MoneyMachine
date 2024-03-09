using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.TelegramConstants;
using Trading.Application.UserInputPipeline;

namespace Trading.Application.Handlers;

internal class AddOrderHandler(IUserInputPipelineBuilder userInputPipelineBuilder) : IHandler
{
    public Triggers Trigger => Triggers.AddOrder;

    public Task<Tuple<string, InlineKeyboardMarkup>> HandleAsync(string userInput)
    {
        userInputPipelineBuilder.BuildAddOrderPipeline();

        return Task.FromResult(new Tuple<string, InlineKeyboardMarkup>(
            @"Enter Order Info:
            Ticker - 'SILVER'
            Direction - BUY/SELL
            Size - 1
            Level - 20
            Stop loss - optional
            Take profit - optional",
             new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)),
                    }
                })));
    }
}