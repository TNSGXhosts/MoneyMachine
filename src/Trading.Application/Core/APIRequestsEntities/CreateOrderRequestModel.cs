
namespace Trading.Application.Core.APIRequestsEntities;

public class CreateOrderRequestModel : CreatePositionRequestModel
{
    public decimal? Level { get; set; }

    public Types Type { get; set; }

    public string GoodTillDate { get; set; }
}