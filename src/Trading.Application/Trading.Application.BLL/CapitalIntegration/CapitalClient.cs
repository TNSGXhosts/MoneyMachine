using System.Net.Http.Json;

using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Options;

using Nito.AsyncEx;

using Trading.Application.BLL.CapitalIntegrationEntities;
using Trading.Application.BLL.Configuration;

namespace Trading.Application.BLL.CapitalIntegration;

public class CapitalClient(IOptions<CapitalIntegrationSettings> capitalIntegrationSettings,
    ILogger<CapitalClient> logger,
    IHttpClientFactory httpClientFactory) : ICapitalClient
{
    private readonly CapitalIntegrationSettings _capitalSettings = capitalIntegrationSettings.Value;

    public bool CreatePosition(CreatePositionEntity createPositionEntity) {
        try {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(HttpMethod.Post, string.Concat(_capitalSettings.BaseUrl, Endpoints.Positions))
            {
                Content = JsonContent.Create(createPositionEntity)
            };

            var response = AsyncContext.Run(() => httpClient.SendAsync(request));
            response.EnsureSuccessStatusCode();

            var createPositionResult = AsyncContext.Run(() => response.Content.ReadFromJsonAsync<BaseResponse>());
            return string.IsNullOrEmpty(createPositionResult?.ErrorCode);
        } catch (Exception e) {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return false;
        }
    }

    public bool CreateOrder(CreateOrderEntity createOrderEntity) {
        try {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(HttpMethod.Post, string.Concat(_capitalSettings.BaseUrl, Endpoints.WorkOrders))
            {
                Content = JsonContent.Create(createOrderEntity)
            };

            var response = AsyncContext.Run(() => httpClient.SendAsync(request));
            response.EnsureSuccessStatusCode();

            var createOrderResult = AsyncContext.Run(() => response.Content.ReadFromJsonAsync<BaseResponse>());
            return string.IsNullOrEmpty(createOrderResult?.ErrorCode);
        } catch (Exception e) {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return false;
        }
    }

    public bool UpdateOrder(string dealId, UpdateOrderEntity updateOrderEntity) {
        try {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(HttpMethod.Put, string.Concat(_capitalSettings.BaseUrl, Endpoints.WorkOrders, "/", dealId))
            {
                Content = JsonContent.Create(updateOrderEntity)
            };

            var response = AsyncContext.Run(() => httpClient.SendAsync(request));
            response.EnsureSuccessStatusCode();

            var updateOrderResult = AsyncContext.Run(() => response.Content.ReadFromJsonAsync<BaseResponse>());
            return string.IsNullOrEmpty(updateOrderResult?.ErrorCode);
        } catch (Exception e) {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return false;
        }
    }

    public bool ClosePosition(string dealId) {
        try {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(
                HttpMethod.Delete,
                string.Concat(_capitalSettings.BaseUrl, Endpoints.Positions, "/", dealId));

            var response = AsyncContext.Run(() => httpClient.SendAsync(request));
            response.EnsureSuccessStatusCode();

            var cancelPositionResult = AsyncContext.Run(() => response.Content.ReadFromJsonAsync<BaseResponse>());
            return string.IsNullOrEmpty(cancelPositionResult?.ErrorCode);
        } catch (Exception e) {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return false;
        }
    }

    public bool CloseOrder(string dealId) {
        try {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(
                HttpMethod.Delete,
                string.Concat(_capitalSettings.BaseUrl, Endpoints.WorkOrders, "/", dealId));

            var response = AsyncContext.Run(() => httpClient.SendAsync(request));
            response.EnsureSuccessStatusCode();

            var cancelOrderResult = AsyncContext.Run(() => response.Content.ReadFromJsonAsync<BaseResponse>());
            return string.IsNullOrEmpty(cancelOrderResult?.ErrorCode);
        } catch (Exception e) {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return false;
        }
    }

    public bool UpdatePosition(string dealId, UpdatePositionEntity updatePositionEntity) {
        try {
            var httpClient = httpClientFactory.CreateClient("capitalIntegration");
            var request = new HttpRequestMessage(HttpMethod.Put, string.Concat(_capitalSettings.BaseUrl, Endpoints.Positions, "/", dealId))
            {
                Content = JsonContent.Create(updatePositionEntity)
            };

            var response = AsyncContext.Run(() => httpClient.SendAsync(request));
            response.EnsureSuccessStatusCode();

            var createPositionResult = AsyncContext.Run(() => response.Content.ReadFromJsonAsync<BaseResponse>());
            return string.IsNullOrEmpty(createPositionResult?.ErrorCode);
        } catch (Exception e) {
            logger.LogError($"{e.Message} - {e.InnerException}");
            return false;
        }
    }
}