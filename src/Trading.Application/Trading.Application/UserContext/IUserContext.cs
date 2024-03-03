using Trading.Application.Core.APIRequestsEntities;

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