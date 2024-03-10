using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Trading.Application.Configuration;
using Trading.Application.Handlers;
using Trading.Application.Presentation;
using Trading.Application.TelegramIntegration;
using Trading.Application.UserContext;
using Trading.Application.UserInputPipeline;

namespace Trading.Application;

public static class PresentationRegistry
{
    public static void RegisterPresentationServices(this IServiceCollection services, IConfigurationRoot configuration)
    {
        ConfigurationRegistry(services, configuration);

        services.AddMemoryCache(options => options.TrackStatistics = true);

        services.AddSingleton<ITelegramClient, TelegramClient>();
        services.AddSingleton<ITradingNotifier, TradingNotifier>();

        services.AddSingleton<IStateProcessor, StateProcessor>();
        services.AddSingleton<IUserContext, UserContext.UserContext>();

        services.AddScoped<IHandlerFactory, HandlerFactory>();

        services.AddScoped<IMessageHandler, MessageHandler>();
        services.AddScoped<ICallbackHandler, CallbackHandler>();
        services.AddScoped<ICallbackParser, CallbackParser>();
        services.AddScoped<ITelegramContext, TelegramContext>();
        services.AddScoped<IUserInputPipelineBuilder, UserInputPipelineBuilder>();
        services.AddScoped<IUserInputPipelineContext, UserInputPipelineContext>();

        RegisterHandlers(services);
    }

    private static void ConfigurationRegistry(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelegramSettings>(settings => configuration.GetSection(nameof(TelegramSettings)).Bind(settings));
    }

    private static void RegisterHandlers(IServiceCollection services)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(IHandler).IsAssignableFrom(p) && !p.IsInterface);
        foreach (var t in types)
        {
            services.AddScoped(typeof(IHandler), t);
        }
    }
}
