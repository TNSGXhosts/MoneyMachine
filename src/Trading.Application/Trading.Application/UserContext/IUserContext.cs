using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.TelegramConstants;

using Trading.Application.UserInputPipeline;

namespace Trading.Application.UserContext;

public interface IUserContext
{
    public States State {get; }
    public string InputString { get;set; }
    public string InputCallback { get;set; }
    public TradeData OrderData { get;set; }
    public InputPipeline UserInputPipeline { get;set; }
    public bool HasPipelineError { get;set; }
    public bool IsMessageExpected { get; }

    public bool ExecuteUserInputPipeline(string input);
    public void CatchEvent(Triggers trigger);
}