namespace Trading.Application.BLL;

public interface IStrategy
{
    bool IsOpenPositionSignal(
        string epic,
        DateTime dateTime = default);

    bool IsClosePositionSignal(
        string epic,
        DateTime dateTime = default);
}
