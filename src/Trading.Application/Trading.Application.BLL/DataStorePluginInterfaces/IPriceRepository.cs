using Core.Models;

using Skender.Stock.Indicators;

namespace Trading.Application.BLL;

public interface IPriceRepository
{
    Task<IEnumerable<PriceEntity>> GetPricesAsync(string ticker, string timeframe, DateTime from, DateTime to);
    Task<IEnumerable<Quote>> GetPricesForStrategyTestAsync(string ticker, string timeframe, DateTime from, DateTime to);

    Task SavePricesAsync(IEnumerable<PriceEntity> prices);
}