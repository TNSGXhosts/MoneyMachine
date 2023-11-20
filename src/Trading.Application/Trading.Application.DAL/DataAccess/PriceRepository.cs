using Trading.Application.DAL.Data;
using Trading.Application.DAL.Models;

namespace Trading.Application.DAL.DataAccess
{
    public class PriceRepository(DataContext dataContext) : IPriceRepository
    {
        public List<Price> GetPrices(string ticker, string timeframe, DateTime from, DateTime to)
        {
            return dataContext.Prices.Where(p => p.Ticker.Equals(ticker, StringComparison.OrdinalIgnoreCase)
                && p.TimeFrame.Equals(timeframe, StringComparison.OrdinalIgnoreCase)
                && DateTime.Parse(p.SnapshotTime) >= from && DateTime.Parse(p.SnapshotTime) <= to).ToList();
        }

        public void SavePrices(List<Price> prices)
        {
            dataContext.AddRange(prices);
        }
    }
}