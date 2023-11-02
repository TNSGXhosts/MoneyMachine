using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Trading.Application.Configuration;
using Trading.Application.TelegramIntegration;

namespace Trading.Application;

public static class PresentationRegistry
{
    public static void ConfigurationRegistry(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.Configure<TelegramSettings>(configuration.GetSection(TelegramSettings.ConfigurationSectionName));
    }

    public static void RegisterPresentationServices(this IServiceCollection services)
    {
        services.AddSingleton<ITelegramClient, TelegramClient>();
    }
}
