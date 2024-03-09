using Core;

namespace Trading.Application.BLL;

public interface IDataManager
{
    Task DownloadAndSavePricesAsync(Timeframe timeframe);
}
