using Core.Models;

using Microsoft.EntityFrameworkCore;

using Trading.Application.BLL;

using Trading.Application.DAL.Data;

namespace Trading.Application.DAL.DataAccess;

internal class PriceRepository(TradingDbContext tradingDbContext) : IPriceRepository
{
    public async Task<IEnumerable<PriceEntity>> GetPricesAsync(string ticker, string timeframe, DateTime from, DateTime to)
    {
        var p = tradingDbContext.Prices.FirstOrDefault();
        return await tradingDbContext.Prices
            .Where(p
                => p.Ticker == ticker
                    && p.TimeFrame == timeframe
                    && p.SnapshotTime >= from && p.SnapshotTime <= to)
            .ToListAsync();
    }

    public async Task SavePrices(List<PriceEntity> prices)
    {
        tradingDbContext.AddRange(prices);
        await tradingDbContext.SaveChangesAsync();
    }
}