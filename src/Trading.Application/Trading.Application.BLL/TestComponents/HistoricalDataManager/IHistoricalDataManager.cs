using Core;

namespace Trading.Application.BLL;

public interface IHistoricalDataManager
{
    Task DownloadAndSavePricesAsync(Timeframe timeframe);
}
