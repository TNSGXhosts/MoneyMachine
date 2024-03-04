namespace Trading.Application.BLL;

public interface IDataManager
{
    Task DownloadAndSavePrices(string epic, string timeframe);
}
