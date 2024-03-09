using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.TelegramConstants;
using Trading.Application.UserInputPipeline;

namespace Trading.Application.Handlers;

internal class AddPositionHandler(IUserInputPipelineBuilder userInputPipelineBuilder) : IHandler
{
    public Triggers Trigger => Triggers.AddPosition;

    public Task<Tuple<string, InlineKeyboardMarkup>> HandleAsync(string userInput)
    {
        userInputPipelineBuilder.BuildAddPositionPipeline();

        return Task.FromResult(new Tuple<string, InlineKeyboardMarkup>(
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
                })));
    }
}