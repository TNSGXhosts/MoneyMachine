using System.Collections;

using Core;

using Skender.Stock.Indicators;

namespace Trading.Application.BLL;

public class StrategyContext : IStrategyContext
{
    public readonly IDictionary<string, Dictionary<DateTime, EpicTestData>> TestData
    = new Dictionary<string, Dictionary<DateTime, EpicTestData>>();

    public int Count
    {
        get => TestData.Count;
    }

    public Dictionary<DateTime, EpicTestData> this[string epic]
    {
        get
        {
            if (TestData.ContainsKey(epic))
            {
                return TestData[epic];
            }

            return null;
        }
    }

    private readonly IPriceRepository _priceRepository;

    public StrategyContext(IPriceRepository priceRepository)
    {
        _priceRepository = priceRepository;
        var unused = GetStrategyDataAsync();
    }

    private async Task GetStrategyDataAsync()
    {
        if (TestData.Count == 0)
        {
            foreach (var epic in StrategyConstants.Coins)
            {
                var prices = await _priceRepository.GetPricesForStrategyTestAsync(
                epic,
                StrategyConstants.Timeframe,
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

                TestData.Add(epic, data);
            }
        }
    }

    public Quote GetAskPrice(string epic, DateTime date)
    {
        if (TestData?.TryGetValue(epic, out var epicTestData) == true && epicTestData?.TryGetValue(date, out var price) == true)
        {
            return price.AskPrice;
        }

        return new Quote();
    }

    public Quote GetBidPrice(string epic, DateTime date)
    {
        if (TestData?.TryGetValue(epic, out var epicTestData) == true && epicTestData?.TryGetValue(date, out var price) == true)
        {
            return price.BidPrice;
        }

        return new Quote();
    }

    public decimal GetSma20(string epic, DateTime date)
    {
        if (TestData?.TryGetValue(epic, out var epicTestData) == true && epicTestData?.TryGetValue(date, out var price) == true)
        {
            var sma = price?.Sma20.Sma;
            return sma != null ? new decimal(sma.Value) : 0m;
        }

        return 0m;
    }

    public decimal GetSma50(string epic, DateTime date)
    {
        if (TestData?.TryGetValue(epic, out var epicTestData) == true && epicTestData?.TryGetValue(date, out var price) == true)
        {
            var sma = price?.Sma50.Sma;
            return sma != null ? new decimal(sma.Value) : 0m;
        }

        return 0m;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach (var pair in TestData)
        {
            yield return pair;
        }
    }

    public IEnumerator<KeyValuePair<string, Dictionary<DateTime, EpicTestData>>> GetEnumerator()
    {
        foreach (var pair in TestData)
        {
            yield return pair;
        }
    }
}
