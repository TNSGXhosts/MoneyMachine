using Trading.Application.Core.APIRequestsEntities;

namespace Core;

public class MarketsModel : BaseResponse
{
    public IEnumerable<Node> Nodes { get;set; }
}

public class Node
{
    public string Id { get;set; }
    public string Name { get; set; }
}