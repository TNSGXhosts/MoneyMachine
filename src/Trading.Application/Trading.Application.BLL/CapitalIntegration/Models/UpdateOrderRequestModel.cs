namespace Trading.Application.BLL.CapitalIntegration.Models;

public class UpdateOrderRequestModel : UpdatePositionRequestModel
{
    public decimal Level { get;set; }

    /// <summary>
    /// YYYY-MM-DDTHH:MM:SS
    /// </summary>
    public string GoodTillDate { get;set; }
}
