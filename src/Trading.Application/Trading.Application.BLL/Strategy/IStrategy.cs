namespace Trading.Application.BLL;

public interface IStrategy
{
    bool IsOpenPositionSignal(
        decimal sma20,
        decimal previousSma20,
        decimal askClosePrice,
        decimal previousAskClosePrice,
        decimal sma50
    );

    bool IsClosePositionSignal(
        decimal sma20,
        decimal previousSma20,
        decimal askClosePrice,
        decimal previousAskClosePrice);
}
