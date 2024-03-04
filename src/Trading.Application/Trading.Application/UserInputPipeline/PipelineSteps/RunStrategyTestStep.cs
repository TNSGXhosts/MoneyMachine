using Core;

using Trading.Application.BLL;
using Trading.Application.UserContext;
using Trading.Application.UserInputPipeline;

namespace Trading.Application;

public class RunStrategyTestStep(IUserContext userContext, IStateProcessor stateProcessor, IDataManager dataManager) : IPipelineStep
{
    public bool Execute(string input)
    {
        dataManager.DownloadAndSavePrices(userContext.OrderData.Epic, nameof(Timeframe.HOUR));

        stateProcessor.CatchEvent(TelegramConstants.Triggers.Start);

        return true;
    }
}
