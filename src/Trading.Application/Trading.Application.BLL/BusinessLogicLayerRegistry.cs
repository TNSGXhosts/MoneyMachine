using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Trading.Application.BLL.Background;
using Trading.Application.BLL.Notifications;
using Trading.Application.BLL.TradingHandler;
using Trading.Application.BLL.TradingInfoCollector;

namespace Trading.Application.BLL;

public static class BusinessLogicLayerRegistry
{
    public static void RegisterBusinessLogicLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigurationRegistry(services, configuration);

        services.AddTransient<ICapitalTradingInfoProcessor, CapitalTradingInfoProcessor>();
        services.AddTransient<ICapitalTradingHandlerProcessor, CapitalTradingHandlerProcessor>();

        services.AddSingleton<ITradingNotificationEvents, TradingNotificationEvents>();

        services.RegisterBackgroundWorkers(configuration);
    }

#pragma warning disable RCS1163 // Unused parameter.
    private static void ConfigurationRegistry(IServiceCollection services, IConfiguration configuration)
#pragma warning restore RCS1163 // Unused parameter.
    {
        // TODO : Add configurations and remove pragma above.
    }
}
