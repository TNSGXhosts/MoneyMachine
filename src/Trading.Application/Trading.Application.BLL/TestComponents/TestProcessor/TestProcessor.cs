using Core;

using Skender.Stock.Indicators;

namespace Trading.Application.BLL;

public class TestProcessor(IPriceRepository priceRepository, IStrategy strategy) : ITestProcessor
{
    private decimal _balance = 10000m;
    private readonly Timeframe _timeframe = Timeframe.DAY;

    private readonly IDictionary<string, Dictionary<DateTime, EpicTestData>> _testData
        = new Dictionary<string, Dictionary<DateTime, EpicTestData>>();

    private readonly decimal _percentOfBalanceForTrade = 0.1m;
    private IList<OpenTestPosition> _openPositionPrices = new List<OpenTestPosition>();

    public async Task<string> Run()
    {
        await GetStrategyDataAsync();

        Test();

        return $"Test result: {_balance}";
    }

    private async Task GetStrategyDataAsync()
    {
        if (_testData.Count == 0)
        {
            foreach (var epic in StrategyConstants.Coins)
            {
                var prices = await priceRepository.GetPricesForStrategyTestAsync(
                epic,
                _timeframe,
                Period.YEAR_5);

                var askPrices = prices.Item1.ToList();
                var bidPrices = prices.Item2.ToList();

                var sma20 = askPrices.GetSma(20).ToList();
                var sma50 = askPrices.GetSma(50).ToList();

                var data = new Dictionary<DateTime, EpicTestData>();
                for (var i = 0; i < askPrices.Count; i++)
                {
                    data.Add(askPrices[i].Date, new EpicTestData
                    {
                        AskPrice = askPrices[i],
                        BidPrice = bidPrices[i],
                        Sma20 = sma20[i],
                        Sma50 = sma50[i],
                    });
                }

                _testData.Add(epic, data);
            }
        }
    }

    private void Test()
    {
        if (_testData.Count > 0)
        {
            foreach(var prices in _testData[StrategyConstants.BTCUSD])
            {
                ClosePositionsIfNecessary(prices.Key);
                OpenPositionIfSignal(prices.Key);
            }
        }
    }

    public IEnumerable<string> ChooseCoinsToTrade(DateTime dateTime)
    {
        var priceChanges = new Dictionary<string, decimal>();

        foreach(var epicData in _testData)
        {
            priceChanges.Add(
                epicData.Key,
                CalculatePercentageChange(
                    epicData.Value.GetAskPrice(dateTime.GetPreviousDate(_timeframe)).Close,
                    epicData.Value.GetAskPrice(dateTime).Close));
        }

        var sortedPriceChanges = priceChanges.OrderByDescending(d => d.Value);
        return sortedPriceChanges.Take(5).Select(d => d.Key);

        return Enumerable.Empty<string>();
    }

    private decimal CalculatePercentageChange(decimal oldPrice, decimal newPrice)
    {
        if (oldPrice == 0)
        {
            return 0;
        }

        return ((newPrice - oldPrice) / oldPrice) * 100;
    }

    public void ClosePositionsIfNecessary(DateTime dateTime)
    {
        var newPositionList = new List<OpenTestPosition>();
        foreach(var price in _openPositionPrices)
        {
            if (strategy.IsClosePositionSignal(
                _testData.GetSma20(price.Epic, dateTime),
                _testData.GetSma20(price.Epic, dateTime.GetPreviousDate(_timeframe)),
                _testData.GetAskPrice(price.Epic, dateTime).Close,
                _testData.GetAskPrice(price.Epic, dateTime.GetPreviousDate(_timeframe)).Close))
            {
                ClosePosition(dateTime, price.Epic, price.OpenPrice, price.OpenVolume);
            } else {
                newPositionList.Add(price);
            }
        }

        _openPositionPrices = newPositionList;
    }

    private void ClosePosition(DateTime dateTime, string epic, decimal openPrice, decimal openVolume)
    {
        var priceChangePercentage = ((_testData.GetBidPrice(epic, dateTime).Close - openPrice) / openPrice) * 100;

        _balance += (openVolume * (100 + priceChangePercentage) / 100) - openVolume;
    }

    public void OpenPositionIfSignal(DateTime dateTime)
    {
        var epics = ChooseCoinsToTrade(dateTime);

        var epicsWithSignals = new Dictionary<string, Quote>();
        foreach(var epic in epics)
        {
            if (strategy.IsOpenPositionSignal(
                _testData.GetSma20(epic, dateTime),
                _testData.GetSma20(epic, dateTime.GetPreviousDate(_timeframe)),
                _testData.GetAskPrice(epic, dateTime).Close,
                _testData.GetAskPrice(epic, dateTime.GetPreviousDate(_timeframe)).Close,
                _testData.GetSma50(StrategyConstants.BTCUSD, dateTime),
                _testData.GetAskPrice(StrategyConstants.BTCUSD, dateTime).Close
                ) && _openPositionPrices.Count < 10)
            {
                epicsWithSignals.Add(epic, _testData[epic][dateTime].AskPrice);
            }
        }

        if (epicsWithSignals.Count > 0)
        {
            var epicToTrade = epicsWithSignals.OrderByDescending(e => e.Value.Volume).First();
            _openPositionPrices.Add(new OpenTestPosition
            {
                Epic = epicToTrade.Key,
                OpenPrice = epicToTrade.Value.Close,
                OpenVolume = _balance * _percentOfBalanceForTrade
            });
        }
    }
}
