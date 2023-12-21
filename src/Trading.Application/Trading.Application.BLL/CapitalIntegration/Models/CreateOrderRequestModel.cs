using Trading.Application.BLL.CapitalIntegration.Enums;

namespace Trading.Application.BLL.CapitalIntegration.Models;

public class CreateOrderRequestModel : CreatePositionRequestModel
{
    public decimal? Level { get; set; }

    public Types Type { get; set; }

    public string GoodTillDate { get; set; }
}