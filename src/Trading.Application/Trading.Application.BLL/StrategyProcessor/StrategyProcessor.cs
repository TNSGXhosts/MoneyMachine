using Core;

using Skender.Stock.Indicators;

namespace Trading.Application.BLL;

public class StrategyProcessor : IStrategyProcessor
{
    private readonly string _epic;
    private readonly IPriceRepository _priceRepository;
    private IList<Quote> _askPrices;
    private IList<Quote> _bidPrices;
    private IList<SmaResult> _sma50;
    private IList<SmaResult> _sma20;

    private decimal _balance;
    private readonly decimal _percentOfBalanceForTrade = 0.05m;
    private decimal? _openPositionPrice = null;

    public StrategyProcessor(string epic, IPriceRepository priceRepository)
    {
        _epic = epic;
        _priceRepository = priceRepository;
    }

    public async void Run()
    {
        var prices = await _priceRepository.GetPricesForStrategyTestAsync(
            _epic,
            Timeframe.DAY,
            Period.YEAR);
        _bidPrices = prices.Item1.ToList();
        _askPrices = prices.Item2.ToList();

        _sma50 = _askPrices.GetSma(50).ToList();
        _sma20 = _askPrices.GetSma(20).ToList();

        _balance = 10000m;

        Test();
    }

    private void Test()
    {
        for (var i = 0; i < _askPrices.Count(); i++)
        {
            if (IsSma50AbovePrice(i) && IsPriceCrossUpSma20(i) && !_openPositionPrice.HasValue)
            {
                _openPositionPrice = _askPrices[i].Close;
            }

            if (IsPriceCrossDownSma20(i) && _openPositionPrice.HasValue)
            {
                var priceChangePercentage = ((_bidPrices[i].Close - _openPositionPrice.Value) / _openPositionPrice.Value) * 100;

                _balance += _balance * _percentOfBalanceForTrade * (100 + priceChangePercentage) / 100;

                _openPositionPrice = null;
            }
        }
    }

    private bool IsSma50AbovePrice(int i)
    {
        return _sma50[i].Sma != null && (decimal)_sma50[i].Sma > _askPrices[i].Close;
    }

    private bool IsPriceCrossUpSma20(int i)
    {
        return (_sma20[i].Sma != null && (decimal)_sma20[i].Sma < _askPrices[i].Close
                && _sma20[i - 1].Sma != null && (decimal)_sma20[i - 1].Sma > _askPrices[i - 1].Close);
    }

    private bool IsPriceCrossDownSma20(int i)
    {
        return (_sma20[i].Sma != null && (decimal)_sma20[i].Sma > _askPrices[i].Close
                && _sma20[i - 1].Sma != null && (decimal)_sma20[i - 1].Sma < _askPrices[i - 1].Close);
    }
}
