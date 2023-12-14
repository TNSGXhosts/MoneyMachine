namespace Trading.Application.BLL.CapitalIntegrationEntities;

public class UpdateOrderEntity : UpdatePositionEntity {
    public double Level { get;set; }
    //YYYY-MM-DDTHH:MM:SS
    public string GoodTillDate { get;set; }
}