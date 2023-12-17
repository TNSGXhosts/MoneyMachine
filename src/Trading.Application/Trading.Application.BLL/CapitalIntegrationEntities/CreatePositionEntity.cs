namespace Trading.Application.BLL.CapitalIntegrationEntities;

public class CreatePositionEntity : UpdatePositionEntity {
    public string Direction { get;set; }
    public string Epic { get;set; }
    public decimal? Size { get; set; }
}