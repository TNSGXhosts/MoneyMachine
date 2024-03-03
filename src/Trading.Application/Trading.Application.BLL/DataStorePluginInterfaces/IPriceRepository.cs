using Core.Models;

namespace Trading.Application.BLL;

public interface IPriceRepository
{
    Task<IEnumerable<PriceEntity>> GetPricesAsync(string ticker, string timeframe, DateTime from, DateTime to);

    Task SavePrices(List<PriceEntity> prices);
}