using Core;
using Core.Models;

using Trading.Application.BLL.CapitalIntegration;

namespace Trading.Application.BLL;

public class DataManager(IPriceRepository priceRepository, IPricesClient pricesClient) : IDataManager
{
    public async Task DownloadAndSavePrices(string epic, Timeframe timeframe)
    {
        const Period period = Period.YEAR;
        var dbPrices = await priceRepository.GetPricesAsync(epic, timeframe, period);

        if (!dbPrices.Any())
        {
            await UpdatePrices(epic, timeframe, period);
        }
    }

    private async Task UpdatePrices(string epic, Timeframe timeframe, Period period)
    {
        var toDate = DateTime.UtcNow.Date.AddDays(-1);
        var fromDate = toDate.AddMonths(-12);

        var newPrices = new List<PriceEntity>(await pricesClient.GetHistoricalPrices(
            epic,
            timeframe,
            CapitalIntegrationConstants.MaxPricesToDownload,
            fromDate,
            toDate
        ));

        var lastPrice = newPrices.LastOrDefault()?.SnapshotTime;
        if (!lastPrice.HasValue)
        {
            throw new Exception("Cannot download prices");
        }

        while (lastPrice < toDate)
        {
            newPrices.Concat(await pricesClient.GetHistoricalPrices(
                epic,
                timeframe,
                CapitalIntegrationConstants.MaxPricesToDownload,
                lastPrice.Value.IncreaseDateByTimeframe(timeframe),
                toDate
            ));

            lastPrice = newPrices.LastOrDefault()?.SnapshotTime;
        }

        var newPriceBatch = new PriceBatch()
        {
            Ticker = epic,
            TimeFrame = timeframe.ToString(),
            Period = period.ToString(),
            Prices = new List<PriceEntity>(newPrices),
            StartDate = fromDate,
            EndDate = toDate
        };

        await priceRepository.SavePriceBatchAsync(newPriceBatch);
    }
}
