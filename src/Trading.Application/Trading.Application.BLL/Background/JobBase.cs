using Microsoft.Extensions.Logging;

using Quartz;

namespace Trading.Application.BLL.Background;

internal class JobBase : IJob
{
    private readonly ILogger _logger;
    private readonly IJobProcessor _processor;

    public JobBase(ILogger logger, IJobProcessor processor)
    {
        _logger = logger;
        _processor = processor;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        if (context.CancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("Job was cancelled");
            return;
        }

        try
        {
            _logger.LogDebug("Execute job");
            await ExecuteAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unhandled exception has occured");
        }
    }

    protected virtual Task ExecuteAsync()
        => _processor.ProcessAsync();
}
