namespace Trading.Application.BLL.CapitalIntegrationEntities;

public class CreateOrderEntity : CreatePositionEntity {
    public double Level { get;set; }
    public string Type { get;set; }
    public string GoodTillDate { get;set; }
}