using Trading.Application.BLL.CapitalIntegrationEntities;

namespace Trading.Application.BLL.CapitalIntegration;

public interface ICapitalClient {
    Task<bool> CreatePosition(CreatePositionEntity createPositionEntity);
    Task<bool> UpdatePosition(string dealId, UpdatePositionEntity updatePositionEntity);
    Task<bool> CreateOrder(CreateOrderEntity createOrderEntity);
    Task<bool> UpdateOrder(string dealId, UpdateOrderEntity updateOrderEntity);
    Task<bool> ClosePosition(string dealId);
    Task<bool> CloseOrder(string dealId);
    Task<IEnumerable<PositionData>> GetPositions();
    Task<IEnumerable<WorkingOrder>> GetOrders();
}