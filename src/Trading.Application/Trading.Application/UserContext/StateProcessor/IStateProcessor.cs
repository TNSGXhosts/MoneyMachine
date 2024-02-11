using Trading.Application.TelegramConstants;

namespace Trading.Application.UserContext;

public interface IStateProcessor
{
    public States State {get; }
    public bool IsMessageExpectedState { get; }

    void CatchEvent(Triggers trigger);
}