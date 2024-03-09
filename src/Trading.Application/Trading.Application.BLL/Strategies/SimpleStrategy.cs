namespace Trading.Application.BLL;

public class SimpleStrategy : IStrategy
{
    public bool IsOpenPositionSignal(
        decimal sma20,
        decimal previousSma20,
        decimal askClosePrice,
        decimal previousAskClosePrice,
        decimal sma50,
        decimal btcAskClosePrice
    )
    {
        return IsSma50AbovePrice(sma50, btcAskClosePrice)
            && IsPriceCrossUpSma20(sma20, previousSma20, askClosePrice, previousAskClosePrice);
    }

    public bool IsClosePositionSignal(
        decimal sma20,
        decimal previousSma20,
        decimal askClosePrice,
        decimal previousAskClosePrice)
    {
        return IsPriceCrossDownSma20(sma20, previousSma20, askClosePrice, previousAskClosePrice);
    }

    private bool IsSma50AbovePrice(decimal sma50, decimal askClosePrice)
    {
        return sma50 > askClosePrice;
    }

    private bool IsPriceCrossUpSma20(
        decimal sma20,
        decimal previousSma20,
        decimal askClosePrice,
        decimal previousAskClosePrice)
    {
        return sma20 < askClosePrice && previousSma20 > previousAskClosePrice;
    }

    private bool IsPriceCrossDownSma20(
        decimal sma20,
        decimal previousSma20,
        decimal askClosePrice,
        decimal previousAskClosePrice)
    {
        return sma20 > askClosePrice && previousSma20 < previousAskClosePrice;
    }
}
