using Core;

using Skender.Stock.Indicators;

namespace Trading.Application.BLL;

public class StrategyProcessor : IStrategyProcessor
{
    private readonly string _epic;
    private readonly IPriceRepository _priceRepository;
    private IEnumerable<Quote> _prices;

    public StrategyProcessor(string epic, IPriceRepository priceRepository)
    {
        _epic = epic;
        _priceRepository = priceRepository;
    }

    public async void Run()
    {
        var toDate = DateTime.UtcNow.Date;
        _prices = await _priceRepository.GetPricesForStrategyTestAsync(
            _epic,
            Timeframe.DAY,
            Period.YEAR);

        var sma = _prices.GetSma(50);
    }
}
