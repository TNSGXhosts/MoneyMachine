using Core;
using Core.Models;

namespace Trading.Application.BLL;

public interface IPricesClient
{
    Task<IEnumerable<PriceEntity>> GetHistoricalPrices(
        string epic,
        Timeframe resolution,
        int? max = null,
        DateTime? from = null,
        DateTime? to = null);
}
