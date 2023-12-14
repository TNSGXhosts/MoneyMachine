using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Trading.Application.Configuration;
using Trading.Application.Handlers;
using Trading.Application.Presentation;
using Trading.Application.TelegramIntegration;
using Trading.Application.UserContext;

namespace Trading.Application;

public static class PresentationRegistry
{
    public static void RegisterPresentationServices(this IServiceCollection services, IConfigurationRoot configuration)
    {
        ConfigurationRegistry(services, configuration);

        services.AddMemoryCache(options => options.TrackStatistics = true);

        services.AddSingleton<ITelegramClient, TelegramClient>();
        services.AddSingleton<ITradingNotifier, TradingNotifier>();

        services.AddSingleton<IUserContext, Context>();

        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(IHandler).IsAssignableFrom(p) && !p.IsInterface);
        foreach (var t in types)
        {
            services.AddScoped(typeof(IHandler), t);
        }
    }

    private static void ConfigurationRegistry(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelegramSettings>(settings => configuration.GetSection(nameof(TelegramSettings)).Bind(settings));
    }
}
