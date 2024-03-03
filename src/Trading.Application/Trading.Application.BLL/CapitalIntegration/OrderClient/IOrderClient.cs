using Trading.Application.Core.APIRequestsEntities;

namespace Trading.Application.BLL.CapitalIntegration;

public interface IOrderClient
{
    Task<bool> CreateOrderAsync(CreateOrderRequestModel requestModel);

    Task<bool> UpdateOrderAsync(string dealId, UpdateOrderRequestModel requestModel);

    Task<bool> CloseOrderAsync(string dealId);

    Task<IEnumerable<WorkingOrder>> GetOrdersAsync();
}
