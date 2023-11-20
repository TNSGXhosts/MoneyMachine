using Microsoft.Extensions.Logging;

using Quartz;

namespace Trading.Application.BLL.Background;

internal class JobBase(ILogger logger, IJobProcessor processor) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        if (context.CancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("Job was cancelled");
            return;
        }

        try
        {
            logger.LogDebug("Execute job");
            await ExecuteAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unhandled exception has occured");
        }
    }

    protected virtual Task ExecuteAsync()
        => processor.ProcessAsync();
}
