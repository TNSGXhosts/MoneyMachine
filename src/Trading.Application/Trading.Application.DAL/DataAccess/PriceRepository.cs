using Microsoft.EntityFrameworkCore;

using Trading.Application.DAL.Data;
using Trading.Application.DAL.Entities;

namespace Trading.Application.DAL.DataAccess
{
    internal class PriceRepository(TradingDbContext tradingDbContext) : IPriceRepository
    {
        public async Task<IEnumerable<PriceEntity>> GetPricesAsync(string ticker, string timeframe, DateTime from, DateTime to)
        {
            return await tradingDbContext.Prices
                .Where(p
                    => p.Ticker.Equals(ticker, StringComparison.OrdinalIgnoreCase)
                       && p.TimeFrame.Equals(timeframe, StringComparison.OrdinalIgnoreCase)
                       // TODO : Resolve the problem with the date time format. Parse is not convertable to SQL and must not be called here.
                       && DateTime.Parse(p.SnapshotTime) >= from && DateTime.Parse(p.SnapshotTime) <= to)
                .ToListAsync();
        }

        public void SavePrices(List<PriceEntity> prices)
        {
            // Why is it not save changes ?
            // TODO : make it async
            tradingDbContext.AddRange(prices);
        }
    }
}