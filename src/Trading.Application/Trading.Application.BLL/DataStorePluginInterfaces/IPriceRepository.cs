using Core;
using Core.Models;

using Skender.Stock.Indicators;

namespace Trading.Application.BLL;

public interface IPriceRepository
{
    Task<IEnumerable<PriceEntity>> GetPricesAsync(string ticker, Timeframe timeframe, Period period);

    Task<(IEnumerable<Quote>, IEnumerable<Quote>)> GetPricesForStrategyTestAsync(
        string ticker,
        Timeframe timeframe,
        Period period);

    Task SavePriceBatchAsync(PriceBatch batch);

    Task<bool> IsBatchExistsAsync(string ticker, Timeframe timeframe, Period period);
}