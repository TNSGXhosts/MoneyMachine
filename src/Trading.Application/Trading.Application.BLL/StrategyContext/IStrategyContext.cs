using Core;

using Skender.Stock.Indicators;

namespace Trading.Application.BLL;

public interface IStrategyContext: IEnumerable<KeyValuePair<string, Dictionary<DateTime, EpicTestData>>>
{
    public int Count { get; }

    Dictionary<DateTime, EpicTestData> this[string epic] { get; }

    Quote GetAskPrice(string epic, DateTime date);
    Quote GetBidPrice(string epic, DateTime date);
    decimal GetSma20(string epic, DateTime date);
    decimal GetSma50(string epic, DateTime date);
}
