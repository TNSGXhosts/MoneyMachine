using Trading.Application.Core.APIRequestsEntities;

namespace Trading.Application.BLL.CapitalIntegration;

public interface IPositionClient
{
    Task<bool> CreatePositionAsync(CreatePositionRequestModel requestModel);
    Task<bool> ClosePositionAsync(string dealId);
    Task<bool> UpdatePositionAsync(string dealId, UpdatePositionRequestModel requestModel);
    Task<IEnumerable<PositionData>> GetPositionsAsync();
}