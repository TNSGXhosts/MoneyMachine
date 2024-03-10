using Skender.Stock.Indicators;

namespace Trading.Application.BLL;

public class PatternStrategy(IStrategyContext strategyContext) : IStrategy
{
    public bool IsClosePositionSignal(string epic, DateTime dateTime = default)
    {
        throw new NotImplementedException();
    }

    public bool IsOpenPositionSignal(string epic, out Quote openPrice, DateTime dateTime = default)
    {
        throw new NotImplementedException();
    }
}
