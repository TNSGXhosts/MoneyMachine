namespace Trading.Application.BLL.CapitalIntegration;

internal static class CapitalIntegrationEndpoints
{
    public const string Positions = "positions";
    public const string WorkOrders = "workingorders";
    public const string Session = "session";
    public const string HistoricalPrices = "prices";
}

internal static class CapitalIntegrationConstants
{
    public const int MaxPricesToDownload = 1000;
}