using Core;

namespace Trading.Application.BLL;

public interface IDataManager
{
    Task DownloadAndSavePrices(string epic, Timeframe timeframe);
}
