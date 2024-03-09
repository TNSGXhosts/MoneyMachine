using Core;

using Skender.Stock.Indicators;

namespace Trading.Application.BLL;

public class TestProcessor(IPriceRepository priceRepository, IStrategy strategy) : ITestProcessor
{
    private decimal _balance = 10000m;
    private readonly IDictionary<string, EpicTestData> _testData = new Dictionary<string, EpicTestData>();
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
                Timeframe.DAY,
                Period.YEAR);

                var askPrices = prices.Item1.ToList();
                var bidPrices = prices.Item2.ToList();

                _testData.Add(epic, new EpicTestData
                {
                    BidPrices = askPrices,
                    AskPrices = bidPrices,
                    Sma20 = askPrices.GetSma(20).ToList(),
                    Sma50 = askPrices.GetSma(50).ToList()
                });
            }
        }
    }

    private void Test()
    {
        if (_testData.Count > 0)
        {
            for (var i = 0; i < _testData.First().Value.AskPrices.Count; i++)
            {
                ClosePositionsIfNecessary(i);
                OpenPositionIfSignal(i);
            }
        }
    }

    public IEnumerable<string> ChooseCoinsToTrade(int i)
    {
        if (i > 0)
        {
            var priceChanges = new Dictionary<string, decimal>();

            foreach(var epicData in _testData)
            {
                priceChanges.Add(
                    epicData.Key,
                    CalculatePercentageChange(epicData.Value.AskPrices[i - 1].Close, epicData.Value.AskPrices[i].Close));
            }

            var sortedPriceChanges = priceChanges.OrderByDescending(d => d.Value);
            return sortedPriceChanges.Take(5).Select(d => d.Key);
        }

        return Enumerable.Empty<string>();
    }

    private decimal CalculatePercentageChange(decimal oldPrice, decimal newPrice)
    {
        return ((newPrice - oldPrice) / oldPrice) * 100;
    }

    public void ClosePositionsIfNecessary(int i)
    {
        var newPositionList = new List<OpenTestPosition>();
        foreach(var price in _openPositionPrices)
        {
            var currentTestData = GetSma20AskCloseByIndex(price.Epic, i);
            var previousTestData = GetSma20AskCloseByIndex(price.Epic, i - 1);

            if (strategy.IsClosePositionSignal(
                currentTestData.Item1,
                previousTestData.Item1,
                currentTestData.Item2,
                previousTestData.Item2))
            {
                ClosePosition(i, price.Epic, price.OpenPrice, price.OpenVolume);
            } else {
                newPositionList.Add(price);
            }
        }

        _openPositionPrices = newPositionList;
    }

    private void ClosePosition(int i, string epic, decimal openPrice, decimal openVolume)
    {
        var priceChangePercentage = ((_testData.GetBidPrice(epic, i).Close - openPrice) / openPrice) * 100;

        _balance += (openVolume * (100 + priceChangePercentage) / 100) - openVolume;
    }

    public void OpenPositionIfSignal(int i)
    {
        var epics = ChooseCoinsToTrade(i);

        var epicsWithSignals = new Dictionary<string, Quote>();
        foreach(var epic in epics)
        {
            var currentTestData = GetSma20AskCloseByIndex(epic, i);
            var previousTestData = GetSma20AskCloseByIndex(epic, i - 1);

            if (strategy.IsOpenPositionSignal(
                currentTestData.Item1,
                previousTestData.Item1,
                currentTestData.Item2,
                previousTestData.Item2,
                (decimal)_testData.GetSma50(epic, i)
                ) && _openPositionPrices.Count < 10)
            {
                epicsWithSignals.Add(epic, _testData.GetAskPrice(epic, i));
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

    private (decimal, decimal) GetSma20AskCloseByIndex(string epic, int i)
    {
        return (_testData.GetSma20(epic, i), _testData.GetAskPrice(epic, i).Close);
    }
}
