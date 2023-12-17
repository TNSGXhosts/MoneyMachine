using System.Net.Http.Json;
using System.Text.Json;

using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Options;

using Trading.Application.BLL.CapitalIntegrationEntities;
using Trading.Application.BLL.Configuration;

namespace Trading.Application.BLL.CapitalIntegration;

public class CapitalClient(IOptions<CapitalIntegrationSettings> capitalIntegrationSettings,
    ILogger<CapitalClient> logger,
    IHttpClientFactory httpClientFactory) : ICapitalClient
{
    private readonly CapitalIntegrationSettings _capitalSettings = capitalIntegrationSettings.Value;

    public async Task<bool> CreatePosition(CreatePositionEntity createPositionEntity)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            };
            var request = new HttpRequestMessage(HttpMethod.Post, string.Concat(_capitalSettings.BaseUrl, Endpoints.Positions))
            {
                Content = JsonContent.Create(createPositionEntity, options: options)
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

    public async Task<bool> CreateOrder(CreateOrderEntity createOrderEntity) {
        try {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            };
            var request = new HttpRequestMessage(HttpMethod.Post, string.Concat(_capitalSettings.BaseUrl, Endpoints.WorkOrders))
            {
                Content = JsonContent.Create(createOrderEntity, options: options)
            };

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var createOrderResult = await response.Content.ReadFromJsonAsync<BaseResponse>();
            return string.IsNullOrEmpty(createOrderResult?.ErrorCode);
        } catch (Exception e) {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return false;
        }
    }

    public async Task<bool> UpdateOrder(string dealId, UpdateOrderEntity updateOrderEntity) {
        try {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(HttpMethod.Put, string.Concat(_capitalSettings.BaseUrl, Endpoints.WorkOrders, "/", dealId))
            {
                Content = JsonContent.Create(updateOrderEntity)
            };

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var updateOrderResult = await response.Content.ReadFromJsonAsync<BaseResponse>();
            return string.IsNullOrEmpty(updateOrderResult?.ErrorCode);
        } catch (Exception e) {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return false;
        }
    }

    public async Task<bool> ClosePosition(string dealId) {
        try {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(
                HttpMethod.Delete,
                string.Concat(_capitalSettings.BaseUrl, Endpoints.Positions, "/", dealId));

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var cancelPositionResult = await response.Content.ReadFromJsonAsync<BaseResponse>();
            return string.IsNullOrEmpty(cancelPositionResult?.ErrorCode);
        } catch (Exception e) {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return false;
        }
    }

    public async Task<bool> CloseOrder(string dealId)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(
                HttpMethod.Delete,
                string.Concat(_capitalSettings.BaseUrl, Endpoints.WorkOrders, "/", dealId));

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

    public async Task<bool> UpdatePosition(string dealId, UpdatePositionEntity updatePositionEntity) {
        try {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(HttpMethod.Put, string.Concat(_capitalSettings.BaseUrl, Endpoints.Positions, "/", dealId))
            {
                Content = JsonContent.Create(updatePositionEntity)
            };

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var createPositionResult = await response.Content.ReadFromJsonAsync<BaseResponse>(cancellationToken: CancellationToken.None);
            return string.IsNullOrEmpty(createPositionResult?.ErrorCode);
        } catch (Exception e) {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return false;
        }
    }

    public async Task<IEnumerable<PositionData>> GetPositions() {
        try {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(HttpMethod.Get, string.Concat(_capitalSettings.BaseUrl, Endpoints.Positions));

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PositionsResponse>();
            return result?.Positions ?? [];
        } catch (Exception e) {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return new List<PositionData>();
        }
    }

    public async Task<IEnumerable<WorkingOrder>> GetOrders() {
        try {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(HttpMethod.Get, string.Concat(_capitalSettings.BaseUrl, Endpoints.WorkOrders));

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<WorkingOrderResponce>();
            return result?.WorkingOrders ?? [];
        } catch (Exception e) {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return new List<WorkingOrder>();
        }
    }
}