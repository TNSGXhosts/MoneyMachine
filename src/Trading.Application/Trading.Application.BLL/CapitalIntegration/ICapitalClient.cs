using Trading.Application.BLL.CapitalIntegration.Models;

namespace Trading.Application.BLL.CapitalIntegration;

public interface ICapitalClient
{
    Task<bool> CreatePositionAsync(CreatePositionRequestModel requestModel);

    Task<bool> UpdatePositionAsync(string dealId, UpdatePositionRequestModel requestModel);

    Task<bool> CreateOrderAsync(CreateOrderRequestModel requestModel);

    Task<bool> UpdateOrderAsync(string dealId, UpdateOrderRequestModel requestModel);

    Task<bool> ClosePositionAsync(string dealId);

    Task<bool> CloseOrderAsync(string dealId);

    Task<IEnumerable<PositionData>> GetPositionsAsync();

    Task<IEnumerable<WorkingOrder>> GetOrdersAsync();
}
