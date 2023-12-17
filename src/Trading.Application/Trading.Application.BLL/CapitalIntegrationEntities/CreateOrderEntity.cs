namespace Trading.Application.BLL.CapitalIntegrationEntities;

public class CreateOrderEntity : CreatePositionEntity {
    public decimal? Level { get;set; }
    public Types Type { get;set; }
    public string GoodTillDate { get;set; }
}