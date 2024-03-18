using Core;

namespace Trading.Application.BLL;

public interface IMarketClient
{
    Task<IEnumerable<Node>> GetTopLevelMarketCategories();
    Task<IEnumerable<Node>> GetAllCategorySubNodes(string nodeId);
}
