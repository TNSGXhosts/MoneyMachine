using Trading.Application.BLL.CapitalIntegrationEntities;

namespace Trading.Application.BLL.CapitalIntegration;

public interface ICapitalClient {
    bool CreatePosition(CreatePositionEntity createPositionEntity);
    bool UpdatePosition(string dealId, UpdatePositionEntity updatePositionEntity);
    bool CreateOrder(CreateOrderEntity createOrderEntity);
    bool UpdateOrder(string dealId, UpdateOrderEntity updateOrderEntity);
    bool ClosePosition(string dealId);
    bool CloseOrder(string dealId);
}