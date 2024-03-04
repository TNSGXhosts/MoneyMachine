using Trading.Application.BLL.CapitalIntegration;

namespace Trading.Application.BLL;

public class DataManager(IPriceRepository priceRepository, IPricesClient pricesClient) : IDataManager
{
    public async Task DownloadAndSavePrices(string epic, string timeframe)
    {
        var fromDate = DateTime.UtcNow.AddMonths(-12);
        var toDate = DateTime.UtcNow;

        var dbPrices = await priceRepository.GetPricesAsync(epic, timeframe, fromDate, toDate);

        if (!dbPrices.Any() || dbPrices.LastOrDefault()?.SnapshotTime < toDate.AddDays(-1))
        {
            var lastDate = dbPrices.LastOrDefault()?.SnapshotTime ?? fromDate;

            await UpdatePrices(epic, timeframe, lastDate, toDate);
        }
    }

    private async Task UpdatePrices(string epic, string timeframe, DateTime from, DateTime to)
    {
        var newPrices = await pricesClient.GetHistoricalPrices(epic, timeframe, CapitalIntegrationConstants.MaxPricesToDownload, from, to);

        foreach (var price in newPrices)
        {
            price.Ticker = epic;
            price.TimeFrame = timeframe;
        }

        await priceRepository.SavePricesAsync(newPrices);
    }
}
