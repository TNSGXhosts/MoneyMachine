namespace Trading.Application.BLL.CapitalIntegration.Models;

public class CreatePositionRequestModel : UpdatePositionRequestModel
{
    public string Direction { get; set; }

    public string Epic { get; set; }

    public decimal? Size { get; set; }
}