namespace Trading.Application.Core.APIRequestsEntities;

public class CreatePositionRequestModel : UpdatePositionRequestModel
{
    public string Direction { get; set; }

    public string Epic { get; set; }

    public decimal? Size { get; set; }
}