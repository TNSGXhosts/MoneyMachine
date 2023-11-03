using System.Net.Http.Json;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Nito.AsyncEx;

using Polly;

using Trading.Application.BLL.Background;
using Trading.Application.BLL.Configuration;
using Trading.Application.BLL.Notifications;
using Trading.Application.BLL.TradingHandler;

namespace Trading.Application.BLL;

public static class BusinessLogicLayerRegistry
{
    public static void RegisterBusinessLogicLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigurationRegistry(services, configuration);

        services.AddTransient<ICapitalTradingHandlerProcessor, CapitalTradingHandlerProcessor>();

        services.AddSingleton<ITradingNotificationEvents, TradingNotificationEvents>();

        services.RegisterBackgroundWorkers(configuration);
        services.RegisterHttpClients();
    }

    private static void ConfigurationRegistry(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CapitalIntegrationSettings>(settings => configuration.GetSection(nameof(CapitalIntegrationSettings))
            .Bind(settings));
    }

    private static void RegisterHttpClients(this IServiceCollection services)
    {
        // TODO : Do something with strings.
        services.AddHttpClient("capitalIntegration", (serviceProvider, httpClient) =>
            {
                var cache = serviceProvider.GetRequiredService<IMemoryCache>();
                var capitalIntegrationSettings = serviceProvider.GetRequiredService<IOptions<CapitalIntegrationSettings>>().Value;

                var accessToken = cache.Get<string>("capitalAccessToken");
                var securityToken = cache.Get<string>("capitalSecurityToken");
                if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(securityToken))
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, capitalIntegrationSettings.BaseUrl + "/session")
                    {
                        Headers =
                        {
                            { "X-CAP-API-KEY", capitalIntegrationSettings.ApiKey }
                        },
                        Content = JsonContent.Create(new
                        {
                            capitalIntegrationSettings.Identifier,
                            capitalIntegrationSettings.Password
                        })
                    };

                    var response = AsyncContext.Run(() => httpClient.SendAsync(request));
                    response.EnsureSuccessStatusCode();

                    var session = AsyncContext.Run(() => response.Content.ReadFromJsonAsync<CapitalIntegrationSession>());
                    accessToken = session!.AccessToken;
                    securityToken = session.SecurityToken;

                    // TODO : Set expiration time.
                    cache.Set("capitalAccessToken", accessToken, new TimeSpan(1, 0, 0));
                    cache.Set("capitalSecurityToken", securityToken, new TimeSpan(1, 0, 0));
                }

                httpClient.DefaultRequestHeaders.Add("CST", accessToken);
                httpClient.DefaultRequestHeaders.Add("X-SECURITY-TOKEN", securityToken);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler { UseCookies = false })
            .AddTransientHttpErrorPolicy(p
                => p.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(300 * retryAttempt)));
    }
}

// TODO : Extract to a separate file.
public class CapitalIntegrationSession
{
    public string AccessToken { get; set; } = string.Empty;

    public string SecurityToken { get; set; } = string.Empty;
}
