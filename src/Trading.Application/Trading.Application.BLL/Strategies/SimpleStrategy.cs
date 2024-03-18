using Core;

namespace Trading.Application.BLL;

public class SimpleStrategy(IStrategyContext strategyContext) : IStrategy
{
    public StrategyType StrategyType => StrategyType.Long;

    public bool IsOpenPositionSignal(
        string epic,
        out decimal openPrice,
        DateTime dateTime = default)
    {
        dateTime = GetCurrentStrategyDate(dateTime);

        var sma50 = strategyContext.GetSma50(StrategyConstants.BTCUSD, dateTime);
        var btcAskClosePrice = strategyContext.GetAskPrice(StrategyConstants.BTCUSD, dateTime).Close;

        var sma20 = strategyContext.GetSma20(epic, dateTime);
        var previousSma20 = strategyContext.GetSma20(epic, dateTime.GetPreviousDate(StrategyConstants.Timeframe));
        var askClosePrice = strategyContext.GetAskPrice(epic, dateTime).Close;
        var previousAskClosePrice = strategyContext.GetAskPrice(epic, dateTime.GetPreviousDate(StrategyConstants.Timeframe)).Close;

        var isSignal = IsSma50AbovePrice(sma50, btcAskClosePrice)
            && IsPriceCrossUpSma20(sma20, previousSma20, askClosePrice, previousAskClosePrice);

        openPrice = !isSignal ? default : strategyContext.GetBidPrice(epic, dateTime).Close;

        return IsSma50AbovePrice(sma50, btcAskClosePrice)
            && IsPriceCrossUpSma20(sma20, previousSma20, askClosePrice, previousAskClosePrice);
    }

    public bool IsClosePositionSignal(
        string epic,
        DateTime openPositionDate,
        out decimal closePrice,
        DateTime dateTime = default)
    {
        dateTime = GetCurrentStrategyDate(dateTime);

        var sma20 = strategyContext.GetSma20(epic, dateTime);
        var previousSma20 = strategyContext.GetSma20(epic, dateTime.GetPreviousDate(StrategyConstants.Timeframe));
        var askClosePrice = strategyContext.GetAskPrice(epic, dateTime).Close;
        var previousAskClosePrice = strategyContext.GetAskPrice(epic, dateTime.GetPreviousDate(StrategyConstants.Timeframe)).Close;

        if (IsPriceCrossDownSma20(sma20, previousSma20, askClosePrice, previousAskClosePrice))
        {
            closePrice = askClosePrice;
            return true;
        }

        closePrice = default;
        return false;
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

    private DateTime GetCurrentStrategyDate(DateTime defaultDate)
    {
        return defaultDate == default ? strategyContext[StrategyConstants.BTCUSD].Last().Value.AskPrice.Date : defaultDate;
    }
}
