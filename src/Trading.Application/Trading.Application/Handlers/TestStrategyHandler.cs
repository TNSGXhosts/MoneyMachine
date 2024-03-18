using Core;

using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.BLL;
using Trading.Application.BLL.Configuration;
using Trading.Application.Handlers;
using Trading.Application.TelegramConstants;

namespace Trading.Application;

public class TestStrategyHandler(
    IHistoricalDataManager dataManager,
    IMarketClient marketClient,
    ITestProcessor testProcessor) : IHandler
{
    public Triggers Trigger => Triggers.TestStrategy;

    public async Task<Tuple<string, InlineKeyboardMarkup>> HandleAsync(string userInput)
    {
        try
        {
            await dataManager.DownloadAndSavePricesAsync(Timeframe.DAY);

            var result = testProcessor.Run();

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
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }
}
