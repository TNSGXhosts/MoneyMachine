using System.Net.Http.Json;

using Core;
using Core.Models;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Trading.Application.BLL.Configuration;

namespace Trading.Application.BLL.CapitalIntegration;

public class PricesClient(
    IOptions<CapitalIntegrationSettings> capitalIntegrationSettings,
    ILogger<PositionClient> logger,
    IHttpClientFactory httpClientFactory) : IPricesClient
{
    private readonly CapitalIntegrationSettings _capitalSettings = capitalIntegrationSettings.Value;

    async Task<IEnumerable<PriceEntity>> IPricesClient.GetHistoricalPrices(
        string epic,
        string resolution = null,
        int? max = null,
        DateTime? from = null,
        DateTime? to = null)
    {
        try
        {
            var uriBuilder = new UriBuilder(_capitalSettings.BaseUrl);
            uriBuilder.Path += CapitalIntegrationEndpoints.HistoricalPrices;
            uriBuilder.Path += $"/{epic}";
            uriBuilder.Query = $"{nameof(resolution)}={resolution}&{nameof(max)}={max}"; //&{nameof(from)}={from}&{nameof(to)}={to}";

            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var prices = await response.Content.ReadFromJsonAsync<HistoricalPrices>();
            if (!string.IsNullOrEmpty(prices?.ErrorCode))
            {
                logger.LogError($"Cannot get prices: {prices.ErrorCode}");
            }

            return prices?.Prices ?? new List<PriceEntity>();
        }
        catch (Exception e)
        {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return new List<PriceEntity>();
        }
    }
}