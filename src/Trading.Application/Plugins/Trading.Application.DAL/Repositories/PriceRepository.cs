using Core.Models;

using Microsoft.EntityFrameworkCore;

using Skender.Stock.Indicators;

using Trading.Application.BLL;

using Trading.Application.DAL.Data;

namespace Trading.Application.DAL.DataAccess;

internal class PriceRepository(TradingDbContext tradingDbContext) : IPriceRepository
{
    public async Task<IEnumerable<PriceEntity>> GetPricesAsync(string ticker, string timeframe, DateTime from, DateTime to)
    {
        return await GetPrices(ticker, timeframe, from, to).ToListAsync();
    }

    public async Task<IEnumerable<Quote>> GetPricesForStrategyTestAsync(string ticker, string timeframe, DateTime from, DateTime to)
    {
        return await GetPrices(ticker, timeframe, from, to).Select(p => new Quote()
        {
            Date = p.SnapshotTime,
            Open = p.OpenPrice.Ask,
            Close = p.ClosePrice.Ask,
            High = p.HighPrice.Ask,
            Low = p.LowPrice.Ask,
            Volume = p.LastTradedVolume
        }).ToListAsync();
    }

    private IQueryable<PriceEntity> GetPrices(string ticker, string timeframe, DateTime from, DateTime to)
    {
        return tradingDbContext.Prices
            .Where(p
                => p.Ticker == ticker
                    && p.TimeFrame == timeframe
                    && p.SnapshotTime >= from && p.SnapshotTime <= to);
    }

    public async Task SavePricesAsync(IEnumerable<PriceEntity> prices)
    {
        tradingDbContext.AddRange(prices);
        await tradingDbContext.SaveChangesAsync();
    }
}