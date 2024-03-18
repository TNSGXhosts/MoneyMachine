using Core;

using Microsoft.Extensions.DependencyInjection;

namespace Trading.Application.BLL;

public class TestProcessor(
    IStrategyContext strategyContext,
    IReportGenerator reportGenerator,
    IServiceProvider serviceProvider,
    ICoinsSeeker coinsSeeker) : ITestProcessor
{
    private decimal _balance = StrategyConstants.StartBalance;
    private readonly Dictionary<string, (decimal, IList<OpenTestPosition>)> _strategiesData = new();

    private readonly decimal _percentOfBalanceForTrade = 0.2m;
    private readonly int _maxOpenPositions = 5;

    public string Run()
    {
        _balance = StrategyConstants.StartBalance;

        using (var scope = serviceProvider.CreateScope())
        {
            var strategies = scope.ServiceProvider.GetServices<IStrategy>();
            foreach (var strategy in strategies)
            {
                _strategiesData.Add(strategy.GetType().Name, (_balance / strategies.Count(), new List<OpenTestPosition>()));
            }
        }

        Test();
        reportGenerator.ExportToExcel();

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

    public void ClosePositionsIfNecessary(DateTime dateTime)
    {
        //var newPositionList = new List<OpenTestPosition>();

        using (var scope = serviceProvider.CreateScope())
        {
            var strategies = scope.ServiceProvider.GetServices<IStrategy>();

            //TODO: fix the same balance for all strategies
            foreach (var strategy in strategies)
            {
                var strategyName = strategy.GetType().Name;
                foreach(var price in _strategiesData[strategyName].Item2.ToList())
                {
                    if (strategy.IsClosePositionSignal(price.Epic, price.OpenDate, out var closePrice, dateTime))
                    {
                        ClosePosition(dateTime, price.Epic, price.OpenPrice, closePrice, price.OpenVolume, strategy);
                        _strategiesData[strategyName].Item2.Remove(price);
                    }
                    // else {
                    //     newPositionList.Add(price);
                    // }
                }
                //_strategiesData[strategyName].Item2 = newPositionList;
            }
        }
    }

    private void ClosePosition(
        DateTime dateTime,
        string epic,
        decimal openPrice,
        decimal closePrice,
        decimal openVolume,
        IStrategy strategy)
    {
        if (openPrice != 0)
        {
            var strategyName = strategy.GetType().Name;

            var priceChangePercentage = ((closePrice - openPrice) / openPrice) * 100;

            var tradeResult = (openVolume * (100 + priceChangePercentage) / 100) - openVolume;
            if (strategy.StrategyType == StrategyType.Short)
            {
                tradeResult = -tradeResult;
            }

            var currentStrategyBalance = _strategiesData[strategyName].Item1;
            currentStrategyBalance += tradeResult;

            _balance += tradeResult;

            _strategiesData[strategyName] = (currentStrategyBalance, _strategiesData[strategyName].Item2);

            reportGenerator.ProceedTrade(
                priceChangePercentage,
                dateTime,
                epic,
                strategyName,
                openPrice,
                closePrice,
                _balance);
        }
    }

    public void OpenPositionIfSignal(DateTime dateTime)
    {
        var epics = coinsSeeker.ChooseCoinsToTrade(dateTime);

        // var epicsWithSignals = new Dictionary<string, decimal>();

        using (var scope = serviceProvider.CreateScope())
        {
            var strategies = scope.ServiceProvider.GetServices<IStrategy>();

            foreach(var strategy in strategies)
            {
                var strategyName = strategy.GetType().Name;
                foreach(var epic in epics)
                {
                    if (strategy.IsOpenPositionSignal(epic, out var openPrice, dateTime)
                        && _strategiesData[strategyName].Item2.Count < _maxOpenPositions)
                    {
                        _strategiesData[strategyName].Item2.Add(new OpenTestPosition
                        {
                            Epic = epic,
                            OpenPrice = openPrice,
                            OpenVolume = _strategiesData[strategyName].Item1 * _percentOfBalanceForTrade,
                            OpenDate = dateTime
                        });
                    }
                }
            }
        }

        // if (epicsWithSignals.Count > 0)
        // {
        //     //TODO: determinate if we need filter signals by Trade Volume
        //     //var epicToTrade = epicsWithSignals.OrderByDescending(e => e.Value.Volume).First();
        //     foreach(var signal in epicsWithSignals)
        //     {
        //         _openPositionPrices.Add(new OpenTestPosition
        //         {
        //             Epic = signal.Key,
        //             OpenPrice = signal.Value,
        //             OpenVolume = _balance * _percentOfBalanceForTrade,
        //             OpenDate = dateTime
        //         });
        //     }
        // }
    }
}
