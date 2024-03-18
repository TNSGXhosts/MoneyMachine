namespace Trading.Application.BLL.CapitalIntegration;

internal static class CapitalIntegrationEndpoints
{
    public const string Positions = "positions";
    public const string WorkOrders = "workingorders";
    public const string Session = "session";
    public const string HistoricalPrices = "prices";
    public const string MarketNavigation = "marketnavigation";
}

internal static class CapitalIntegrationConstants
{
    public const int MaxPricesToDownload = 1000;
    public const string CryptoCurrenciesNode = "crypto_currencies_group";
}