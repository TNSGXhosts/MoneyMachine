namespace Trading.Application.BLL.CapitalIntegrationEntities;

public class CreatePositionEntity : UpdatePositionEntity {
    public Directions Direction { get;set; }
    public required string Epic { get;set; }
    public double Size { get; set; }
}