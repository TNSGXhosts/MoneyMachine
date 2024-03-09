namespace Trading.Application.BLL;

public class PatternStrategy(IStrategyContext strategyContext) : IStrategy
{
    public bool IsClosePositionSignal(decimal sma20, decimal previousSma20, decimal askClosePrice, decimal previousAskClosePrice)
    {
        throw new NotImplementedException();
    }

    public bool IsOpenPositionSignal(
        decimal sma20,
        decimal previousSma20,
        decimal askClosePrice,
        decimal previousAskClosePrice,
        decimal sma50,
        decimal btcAskClosePrice)
    {
        throw new NotImplementedException();
    }
}
