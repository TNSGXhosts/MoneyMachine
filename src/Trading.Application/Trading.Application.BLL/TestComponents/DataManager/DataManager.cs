using Core;
using Core.Models;

using Trading.Application.BLL.CapitalIntegration;

namespace Trading.Application.BLL;

public class DataManager(IPriceRepository priceRepository, IPricesClient pricesClient) : IDataManager
{
    public async Task DownloadAndSavePricesAsync(Timeframe timeframe)
    {
        const Period period = Period.YEAR;

        foreach (var epic in StrategyConstants.Coins)
        {
            var isExists = await priceRepository.IsBatchExistsAsync(epic, timeframe, period);

            if (!isExists)
            {
                await DownloadPricesAsync(epic, timeframe, period);
            }
        }
    }

    private async Task DownloadPricesAsync(string epic, Timeframe timeframe, Period period)
    {
        var periodDates = period.GetPeriod();

        var newPrices = new List<PriceEntity>(await pricesClient.GetHistoricalPrices(
            epic,
            timeframe,
            CapitalIntegrationConstants.MaxPricesToDownload,
            periodDates.Item1
        ));

        var lastPrice = newPrices.LastOrDefault()?.SnapshotTime;
        if (!lastPrice.HasValue)
        {
            throw new Exception("Cannot download prices");
        }

        while (lastPrice < periodDates.Item2)
        {
            newPrices.AddRange(await pricesClient.GetHistoricalPrices(
                epic,
                timeframe,
                CapitalIntegrationConstants.MaxPricesToDownload,
                lastPrice.Value.IncreaseDateByTimeframe(timeframe)
            ));

            lastPrice = newPrices.LastOrDefault()?.SnapshotTime;
        }

        var newPriceBatch = new PriceBatch()
        {
            Ticker = epic,
            TimeFrame = timeframe.ToString(),
            Period = period.ToString(),
            Prices = new List<PriceEntity>(newPrices),
            StartDate = periodDates.Item1,
            EndDate = periodDates.Item2
        };

        await priceRepository.SavePriceBatchAsync(newPriceBatch);
    }
}
