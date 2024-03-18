using Core;

namespace Trading.Application.BLL;

public interface IReportGenerator
{
    void ProceedTrade(
        decimal priceChangePercentage,
        DateTime closeDateTime,
        string epic,
        string strategy,
        decimal openPrice,
        decimal closePrice,
        decimal currentBalance);

    void ExportToExcel();
}
