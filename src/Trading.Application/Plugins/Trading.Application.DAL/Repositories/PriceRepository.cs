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
        var prices = await GetPriceBatch(ticker, timeframe, period);
        return prices;
    }

    public async Task<IEnumerable<Quote>> GetPricesForStrategyTestAsync(string ticker, Timeframe timeframe, Period period)
    {
        var prices = await GetPriceBatch(ticker, timeframe, period);
        return prices.Select(p => new Quote()
        {
            Date = p.SnapshotTime,
            Open = p.OpenPrice.Bid,
            Close = p.ClosePrice.Bid,
            High = p.HighPrice.Bid,
            Low = p.LowPrice.Bid,
            Volume = p.LastTradedVolume
        });
    }

    private async Task<IEnumerable<PriceEntity>> GetPriceBatch(string ticker, Timeframe timeframe, Period period)
    {
        var priceBatch = await tradingDbContext.PriceBatches
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
}