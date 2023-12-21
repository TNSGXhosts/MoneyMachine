using Trading.Application.DAL.Entities;

namespace Trading.Application.DAL.DataAccess
{
    public interface IPriceRepository
    {
        Task<IEnumerable<PriceEntity>> GetPricesAsync(string ticker, string timeframe, DateTime from, DateTime to);

        void SavePrices(List<PriceEntity> prices);
    }
}