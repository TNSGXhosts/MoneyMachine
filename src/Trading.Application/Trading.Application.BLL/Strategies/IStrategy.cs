using Skender.Stock.Indicators;

namespace Trading.Application.BLL;

public interface IStrategy
{
    bool IsOpenPositionSignal(
        string epic,
        out Quote openPrice,
        DateTime dateTime = default);

    bool IsClosePositionSignal(
        string epic,
        DateTime dateTime = default);
}
