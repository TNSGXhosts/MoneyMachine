using Trading.Application.BLL.CapitalIntegration.Models;
using Trading.Application.TelegramConstants;

using Trading.Application.UserInputPipeline;

namespace Trading.Application.UserContext;

public interface IUserContext
{
    public string InputCallback { get;set; }
    public TradeData OrderData { get;set; }
    public bool HasPipelineError { get;set; }
    public bool IsMessageExpected { get; }
    public PositionData PositionData { get;set; }
    public WorkingOrder WorkingOrder { get;set; }
}