using System.Net.Http.Json;
using System.Text.Json;

using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Options;

using Trading.Application.BLL.CapitalIntegration.Models;
using Trading.Application.BLL.Configuration;

namespace Trading.Application.BLL.CapitalIntegration;

public class CapitalClient(
    IOptions<CapitalIntegrationSettings> capitalIntegrationSettings,
    ILogger<CapitalClient> logger,
    IHttpClientFactory httpClientFactory) : ICapitalClient
{
    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    };

    private readonly CapitalIntegrationSettings _capitalSettings = capitalIntegrationSettings.Value;

    public async Task<bool> CreatePositionAsync(CreatePositionRequestModel requestModel)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(HttpMethod.Post,
                string.Concat(_capitalSettings.BaseUrl, CapitalIntegrationEndpoints.Positions))
            {
                Content = JsonContent.Create(requestModel, options: s_jsonOptions)
            };

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var createPositionResult = await response.Content.ReadFromJsonAsync<BaseResponse>();
            return string.IsNullOrEmpty(createPositionResult?.ErrorCode);
        }
        catch (Exception e)
        {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return false;
        }
    }

    public async Task<bool> CreateOrderAsync(CreateOrderRequestModel requestModel)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(HttpMethod.Post,
                string.Concat(_capitalSettings.BaseUrl, CapitalIntegrationEndpoints.WorkOrders))
            {
                Content = JsonContent.Create(requestModel, options: s_jsonOptions)
            };

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var createOrderResult = await response.Content.ReadFromJsonAsync<BaseResponse>();
            return string.IsNullOrEmpty(createOrderResult?.ErrorCode);
        }
        catch (Exception e)
        {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return false;
        }
    }

    public async Task<bool> UpdateOrderAsync(string dealId, UpdateOrderRequestModel requestModel)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var request = new HttpRequestMessage(HttpMethod.Put,
                string.Concat(_capitalSettings.BaseUrl, CapitalIntegrationEndpoints.WorkOrders, "/", dealId))
            {
                Content = JsonContent.Create(requestModel, options: options)
            };

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var updateOrderResult = await response.Content.ReadFromJsonAsync<BaseResponse>();
            return string.IsNullOrEmpty(updateOrderResult?.ErrorCode);
        }
        catch (Exception e)
        {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return false;
        }
    }

    public async Task<bool> ClosePositionAsync(string dealId)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(
                HttpMethod.Delete,
                string.Concat(_capitalSettings.BaseUrl, CapitalIntegrationEndpoints.Positions, "/", dealId));

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var cancelPositionResult = await response.Content.ReadFromJsonAsync<BaseResponse>();
            return string.IsNullOrEmpty(cancelPositionResult?.ErrorCode);
        }
        catch (Exception e)
        {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return false;
        }
    }

    public async Task<bool> CloseOrderAsync(string dealId)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(
                HttpMethod.Delete,
                string.Concat(_capitalSettings.BaseUrl, CapitalIntegrationEndpoints.WorkOrders, "/", dealId));

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var cancelOrderResult = await response.Content.ReadFromJsonAsync<BaseResponse>();

            return string.IsNullOrEmpty(cancelOrderResult?.ErrorCode);
        }
        catch (Exception e)
        {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return false;
        }
    }

    public async Task<bool> UpdatePositionAsync(string dealId, UpdatePositionRequestModel requestModel)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var request = new HttpRequestMessage(HttpMethod.Put,
                string.Concat(_capitalSettings.BaseUrl, CapitalIntegrationEndpoints.Positions, "/", dealId))
            {
                Content = JsonContent.Create(requestModel, options: options)
            };

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var createPositionResult = await response.Content.ReadFromJsonAsync<BaseResponse>(cancellationToken: CancellationToken.None);
            return string.IsNullOrEmpty(createPositionResult?.ErrorCode);
        }
        catch (Exception e)
        {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return false;
        }
    }

    public async Task<IEnumerable<PositionData>> GetPositionsAsync()
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(HttpMethod.Get,
                string.Concat(_capitalSettings.BaseUrl, CapitalIntegrationEndpoints.Positions));

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PositionsResponse>();
            return result?.Positions ?? [];
        }
        catch (Exception e)
        {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return new List<PositionData>();
        }
    }

    public async Task<IEnumerable<WorkingOrder>> GetOrdersAsync()
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(HttpMethod.Get,
                string.Concat(_capitalSettings.BaseUrl, CapitalIntegrationEndpoints.WorkOrders));

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<WorkingOrderResponce>();
            return result?.WorkingOrders ?? [];
        }
        catch (Exception e)
        {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return new List<WorkingOrder>();
        }
    }
}