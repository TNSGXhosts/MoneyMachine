using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.Handlers;
using Trading.Application.TelegramConstants;
using Trading.Application.UserInputPipeline;

namespace Trading.Application;

public class TestStrategyHandler(IUserInputPipelineBuilder userInputPipelineBuilder) : IHandler
{
    public Triggers Trigger => Triggers.TestStrategy;

    public Tuple<string, InlineKeyboardMarkup> Handle(string userInput)
    {
        userInputPipelineBuilder.BuildTestStrategyPipeline();

        return new Tuple<string, InlineKeyboardMarkup>(
            @"Enter Test Data:
            Ticker - 'SILVER'",
             new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)),
                    }
                }));
    }
}
