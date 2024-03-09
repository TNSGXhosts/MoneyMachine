using Core;
using Core.Models;

using Microsoft.EntityFrameworkCore;

using Skender.Stock.Indicators;

using Trading.Application.BLL;

using Trading.Application.DAL.Data;

namespace Trading.Application.DAL.DataAccess;

internal class PriceRepository(TradingDbContext tradingDbContext) : IPriceRepository
{
    public async Task<IEnumerable<PriceEntity>> GetPricesAsync(string ticker, Timeframe timeframe, Period period)
    {
        var prices = await GetPriceBatchAsync(ticker, timeframe, period);
        return prices;
    }

    public async Task<(IEnumerable<Quote>, IEnumerable<Quote>)> GetPricesForStrategyTestAsync(
        string ticker,
        Timeframe timeframe,
        Period period)
    {
        var prices = await GetPriceBatchAsync(ticker, timeframe, period);
        var bidPrices = prices.Select(p => new Quote()
        {
            Date = p.SnapshotTime,
            Open = p.OpenPrice.Bid,
            Close = p.ClosePrice.Bid,
            High = p.HighPrice.Bid,
            Low = p.LowPrice.Bid,
            Volume = p.LastTradedVolume
        });
        var askPrices = prices.Select(p => new Quote()
        {
            Date = p.SnapshotTime,
            Open = p.OpenPrice.Ask,
            Close = p.ClosePrice.Ask,
            High = p.HighPrice.Bid,
            Low = p.LowPrice.Bid,
            Volume = p.LastTradedVolume
        });

        return (bidPrices, askPrices);
    }

    private async Task<IEnumerable<PriceEntity>> GetPriceBatchAsync(string ticker, Timeframe timeframe, Period period)
    {
        var priceBatch = await tradingDbContext.PriceBatches
            .Include(b => b.Prices).ThenInclude(p => p.HighPrice)
            .Include(b => b.Prices).ThenInclude(p => p.LowPrice)
            .Include(b => b.Prices).ThenInclude(p => p.ClosePrice)
            .Include(b => b.Prices).ThenInclude(p => p.OpenPrice)
            .FirstOrDefaultAsync(p
                => p.Ticker == ticker
                    && p.TimeFrame == timeframe.ToString() && p.Period == period.ToString());

        return priceBatch?.Prices ?? Enumerable.Empty<PriceEntity>();
    }

    public async Task SavePriceBatchAsync(PriceBatch batch)
    {
        tradingDbContext.PriceBatches.Add(batch);
        await tradingDbContext.SaveChangesAsync();
    }

    public async Task<bool> IsBatchExistsAsync(string ticker, Timeframe timeframe, Period period)
    {
        var exists = await tradingDbContext.PriceBatches
            .AnyAsync(p
                => p.Ticker == ticker
                    && p.TimeFrame == timeframe.ToString() && p.Period == period.ToString());

        return exists;
    }
}