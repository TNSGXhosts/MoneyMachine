namespace Trading.Application.Core.APIRequestsEntities;

public class UpdatePositionRequestModel
{
    /// <summary>
    /// Must be true if a guaranteed stop is required (cannot be true if TrailingStop = true)
    /// </summary>
    public bool GuaranteedStop { get; set; }

    /// <summary>
    /// Must be true if a trailing stop is required
    /// </summary>
    public bool TrailingStop { get; set; }

    public decimal? StopLevel { get; set; }

    // public decimal StopDistance { get;set; }
    // public decimal StopAmount { get;set; }

    public decimal? ProfitLevel { get; set; }

    // public decimal ProfitDistance { get;set; }
    // public decimal ProfitAmount { get;set; }
}
