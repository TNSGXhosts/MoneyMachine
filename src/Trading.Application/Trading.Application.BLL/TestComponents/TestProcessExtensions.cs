using Core;

using Skender.Stock.Indicators;

namespace Trading.Application.BLL;

public static class TestProcessExtensions
{
    public static Quote GetAskPrice(this IDictionary<string, EpicTestData> epics, string epic, int index)
    {
        if (epics?.TryGetValue(epic, out var epicTestData) == true && index >= 0 && index < epicTestData.AskPrices.Count)
        {
            return epicTestData.BidPrices[index];
        }

        return new Quote();
    }

    public static Quote GetBidPrice(this IDictionary<string, EpicTestData> epics, string epic, int index)
    {
        if (epics?.TryGetValue(epic, out var epicTestData) == true && index >= 0 && index < epicTestData.BidPrices.Count)
        {
            return epicTestData.BidPrices[index];
        }

        return new Quote();
    }

    public static decimal GetSma20(this IDictionary<string, EpicTestData>? epics, string epic, int index)
    {
        if (epics?.TryGetValue(epic, out var epicTestData) == true && index >= 0 && index < epicTestData.Sma20.Count)
        {
            var sma = epicTestData?.Sma20?[index]?.Sma;
            return sma != null ? new decimal(sma.Value) : 0m;
        }

        return 0m;
    }

    public static decimal GetSma50(this IDictionary<string, EpicTestData> epics, string epic, int index)
    {
        if (epics?.TryGetValue(epic, out var epicTestData) == true && index >= 0 && index < epicTestData.Sma50.Count)
        {
            var sma = epicTestData?.Sma50?[index]?.Sma;
            return sma != null ? new decimal(sma.Value) : 0m;
        }

        return 0m;
    }
}
