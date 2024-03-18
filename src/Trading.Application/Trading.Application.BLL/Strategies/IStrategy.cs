using Core;

namespace Trading.Application.BLL;

public interface IStrategy
{
    public StrategyType StrategyType { get; }

    bool IsOpenPositionSignal(
        string epic,
        out decimal openPrice,
        DateTime dateTime = default);

    bool IsClosePositionSignal(
        string epic,
        DateTime openPositionDate,
        out decimal closePrice,
        DateTime dateTime = default);
}
