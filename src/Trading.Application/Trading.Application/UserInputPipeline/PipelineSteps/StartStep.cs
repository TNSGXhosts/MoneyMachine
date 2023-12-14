using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.TelegramConstants;
using Trading.Application.TelegramIntegration;

namespace Trading.Application.UserInputPipeline;

public class StartStep(ITelegramClient telegramClient) : IPipelineStep
{
    public bool Execute(string input)
    {
        if (input != nameof(Triggers.Start))
        {
            return false;
        }

        telegramClient.SendMessageAsync("Choose an action:",
            new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Create Order", nameof(Triggers.AddOrder)),
                        InlineKeyboardButton.WithCallbackData("Create Position", nameof(Triggers.AddPosition)),
                        InlineKeyboardButton.WithCallbackData("Choose Order", nameof(Triggers.ChooseOrder)),
                        InlineKeyboardButton.WithCallbackData("Choose Position", nameof(Triggers.ChoosePosition)),
                    }
                }));

        return true;
    }
}