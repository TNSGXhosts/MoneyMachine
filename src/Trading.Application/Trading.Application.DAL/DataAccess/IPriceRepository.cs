using Trading.Application.DAL.Models;

namespace Trading.Application.DAL.DataAccess
{
    public interface IPriceRepository
    {
        List<Price> GetPrices(string ticker, string timeframe, DateTime from, DateTime to);
        void SavePrices(List<Price> prices);
    }
}