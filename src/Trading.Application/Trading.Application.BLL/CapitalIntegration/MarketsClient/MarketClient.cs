using System.Net.Http.Json;

using Core;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Trading.Application.BLL.CapitalIntegration;
using Trading.Application.BLL.Configuration;

namespace Trading.Application.BLL;

public class MarketClient(
    IOptions<CapitalIntegrationSettings> capitalIntegrationSettings,
    ILogger<PositionClient> logger,
    IHttpClientFactory httpClientFactory) : IMarketClient
{
    private readonly CapitalIntegrationSettings _capitalSettings = capitalIntegrationSettings.Value;

    public async Task<IEnumerable<Node>> GetTopLevelMarketCategories()
    {
        try
        {
            var uriBuilder = new UriBuilder(_capitalSettings.BaseUrl);
            uriBuilder.Path += CapitalIntegrationEndpoints.MarketNavigation;

            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var prices = await response.Content.ReadFromJsonAsync<MarketsModel>();
            if (!string.IsNullOrEmpty(prices?.ErrorCode))
            {
                logger.LogError($"Cannot get market top categories: {prices.ErrorCode}");
            }

            return prices?.Nodes ?? new List<Node>();
        }
        catch (Exception e)
        {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return new List<Node>();
        }
    }

    public async Task<IEnumerable<Node>> GetAllCategorySubNodes(string nodeId)
    {
        try
        {
            var uriBuilder = new UriBuilder(_capitalSettings.BaseUrl);
            uriBuilder.Path += CapitalIntegrationEndpoints.MarketNavigation;
            uriBuilder.Path += $"/{nodeId}";

            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var prices = await response.Content.ReadFromJsonAsync<MarketsModel>();
            if (!string.IsNullOrEmpty(prices?.ErrorCode))
            {
                logger.LogError($"Cannot get market categories: {prices.ErrorCode}");
            }

            return prices?.Nodes ?? new List<Node>();
        }
        catch (Exception e)
        {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return new List<Node>();
        }
    }
}
