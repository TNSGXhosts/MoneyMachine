using Core;

using Skender.Stock.Indicators;

namespace Trading.Application.BLL;

public class TestProcessor(IStrategy strategy, IStrategyContext strategyContext) : ITestProcessor
{
    private decimal _balance = StrategyConstants.StartBalance;

    private readonly decimal _percentOfBalanceForTrade = 0.1m;
    private IList<OpenTestPosition> _openPositionPrices = new List<OpenTestPosition>();

    public async Task<string> Run()
    {
        _balance = StrategyConstants.StartBalance;
        _openPositionPrices = new List<OpenTestPosition>();

        Test();

        return $"Test result: {_balance}";
    }

    private void Test()
    {
        if (strategyContext.Count > 0)
        {
            foreach(var prices in strategyContext[StrategyConstants.BTCUSD])
            {
                ClosePositionsIfNecessary(prices.Key);
                OpenPositionIfSignal(prices.Key);
            }
        }
    }

    public IEnumerable<string> ChooseCoinsToTrade(DateTime dateTime)
    {
        var priceChanges = new Dictionary<string, decimal>();

        foreach(var epicData in strategyContext)
        {
            priceChanges.Add(
                epicData.Key,
                CalculatePercentageChange(
                    epicData.Value.GetAskPrice(dateTime.GetPreviousDate(StrategyConstants.Timeframe)).Close,
                    epicData.Value.GetAskPrice(dateTime).Close));
        }

        var sortedPriceChanges = priceChanges.OrderByDescending(d => d.Value);
        return sortedPriceChanges.Take(5).Select(d => d.Key);
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
                strategyContext.GetSma20(price.Epic, dateTime),
                strategyContext.GetSma20(price.Epic, dateTime.GetPreviousDate(StrategyConstants.Timeframe)),
                strategyContext.GetAskPrice(price.Epic, dateTime).Close,
                strategyContext.GetAskPrice(price.Epic, dateTime.GetPreviousDate(StrategyConstants.Timeframe)).Close))
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
        var priceChangePercentage = ((strategyContext.GetBidPrice(epic, dateTime).Close - openPrice) / openPrice) * 100;

        _balance += (openVolume * (100 + priceChangePercentage) / 100) - openVolume;
    }

    public void OpenPositionIfSignal(DateTime dateTime)
    {
        var epics = ChooseCoinsToTrade(dateTime);

        var epicsWithSignals = new Dictionary<string, Quote>();
        foreach(var epic in epics)
        {
            if (strategy.IsOpenPositionSignal(
                strategyContext.GetSma20(epic, dateTime),
                strategyContext.GetSma20(epic, dateTime.GetPreviousDate(StrategyConstants.Timeframe)),
                strategyContext.GetAskPrice(epic, dateTime).Close,
                strategyContext.GetAskPrice(epic, dateTime.GetPreviousDate(StrategyConstants.Timeframe)).Close,
                strategyContext.GetSma50(StrategyConstants.BTCUSD, dateTime),
                strategyContext.GetAskPrice(StrategyConstants.BTCUSD, dateTime).Close
                ) && _openPositionPrices.Count < 10)
            {
                epicsWithSignals.Add(epic, strategyContext.GetAskPrice(epic, dateTime));
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
