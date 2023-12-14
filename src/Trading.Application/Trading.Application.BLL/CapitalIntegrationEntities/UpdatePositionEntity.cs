namespace Trading.Application.BLL.CapitalIntegrationEntities;

public class UpdatePositionEntity {
    //Must be true if a guaranteed stop is required (cannot be true if TrailingStop = true)
    public bool GuaranteedStop { get;set; }
    //Must be true if a trailing stop is required
    public bool TrailingStop { get;set; }
    public double StopLevel { get;set; }
    public double StopDistance { get;set; }
    public double StopAmount { get;set; }
    public double ProfitLevel { get;set; }
    public double ProfitDistance { get;set; }
    public double ProfitAmount { get;set; }
}