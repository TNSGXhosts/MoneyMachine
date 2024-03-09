using Core;

using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.BLL;
using Trading.Application.Handlers;
using Trading.Application.TelegramConstants;

namespace Trading.Application;

public class TestStrategyHandler(
    IDataManager dataManager,
    ITestProcessor testProcessor) : IHandler
{
    public Triggers Trigger => Triggers.TestStrategy;

    public async Task<Tuple<string, InlineKeyboardMarkup>> HandleAsync(string userInput)
    {
        await dataManager.DownloadAndSavePricesAsync(Timeframe.DAY);

        var result = await testProcessor.Run();

        return new Tuple<string, InlineKeyboardMarkup>(
            result,
             new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)),
                    }
                }));
    }
}
