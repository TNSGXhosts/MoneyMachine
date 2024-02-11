
using Trading.Application.BLL.CapitalIntegration.Models;

namespace Trading.Application.UserContext;

public class UserContext(IStateProcessor stateProcessor) : IUserContext
{
    public bool HasPipelineError { get; set; }

    public string InputCallback { get; set; }

    public TradeData OrderData { get; set; }

    bool IUserContext.IsMessageExpected => stateProcessor.IsMessageExpectedState;

    public PositionData PositionData { get; set; }

    public WorkingOrder WorkingOrder { get; set; }
}