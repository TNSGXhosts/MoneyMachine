using Core;

using Skender.Stock.Indicators;

namespace Trading.Application.BLL;

public static class TestProcessExtensions
{
    public static Quote GetAskPrice(this IDictionary<string, Dictionary<DateTime, EpicTestData>> epics, string epic, DateTime date)
    {
        if (epics?.TryGetValue(epic, out var epicTestData) == true && epicTestData?.TryGetValue(date, out var price) == true)
        {
            return price.AskPrice;
        }

        return new Quote();
    }

    public static Quote GetBidPrice(this IDictionary<string, Dictionary<DateTime, EpicTestData>> epics, string epic, DateTime date)
    {
        if (epics?.TryGetValue(epic, out var epicTestData) == true && epicTestData?.TryGetValue(date, out var price) == true)
        {
            return price.BidPrice;
        }

        return new Quote();
    }

    public static decimal GetSma20(this IDictionary<string, Dictionary<DateTime, EpicTestData>> epics, string epic, DateTime date)
    {
        if (epics?.TryGetValue(epic, out var epicTestData) == true && epicTestData?.TryGetValue(date, out var price) == true)
        {
            var sma = price?.Sma20.Sma;
            return sma != null ? new decimal(sma.Value) : 0m;
        }

        return 0m;
    }

    public static decimal GetSma50(this IDictionary<string, Dictionary<DateTime, EpicTestData>> epics, string epic, DateTime date)
    {
        if (epics?.TryGetValue(epic, out var epicTestData) == true && epicTestData?.TryGetValue(date, out var price) == true)
        {
            var sma = price?.Sma50.Sma;
            return sma != null ? new decimal(sma.Value) : 0m;
        }

        return 0m;
    }

    public static DateTime GetPreviousDate(this DateTime dateTime, Timeframe timeframe)
    {
        switch (timeframe)
        {
            case Timeframe.HOUR:
                return dateTime.AddHours(-1);
            case Timeframe.DAY:
                return dateTime.AddDays(-1);
            default:
                throw new ArgumentException("Invalid timeframe value", nameof(timeframe));
        }
    }

    public static Quote GetAskPrice(this Dictionary<DateTime, EpicTestData> epicData, DateTime date)
    {
        if (epicData?.TryGetValue(date, out var price) == true)
        {
            return price.AskPrice;
        }

        return new Quote();
    }
}
