using System;
namespace MoneyMachine.Constants
{
    public class ApplicationConstants
    {
        public const string BaseAPIUrl = "https://demo-api-capital.backend-capital.com/api/v1";
        public const string WebSocketUrl = "wss://api-streaming-capital.backend-capital.com/connect";

        public const string DefaultDateFormat = "YYYY-MM-DDTHH:MM:SS";
    }

    public class Fields
    {
        public const string BollingerBandUpper = "Bollinger Band Upper";
        public const string BollingerBandLower = "Bollinger Band Lower";
        public const string RSI = "RSI";
        public const string BID = "bid";
        public const string Balance = "Balance";
        public const string SellSignal = "Sell signal";
        public const string BuySignal = "Buy Signal";
        public const string ProductPriceChanging = "Product Price Changing";
        public const string BalanceChanging = "Balance Changing";
    }

    public class EndPoints
    {
        public const string Accounts = "accounts";
        public const string Session = "/session";
        public const string Positions = "positions";
        public const string Confirms = "confirms";
        public const string Markets = "markets";
        public const string WorkingOrders = "workingorders";
        public const string MarketNavigation = "marketnavigation";
        public const string Prices = "prices";
        public const string ClientSentiment = "clientsentiment";
        public const string Watchlists = "watchlists";
    }

    public class Headers
    {
        public const string APIKey = "X-CAP-API-KEY";
        public const string ContentType = "Content-Type";
        public const string AppJson = "application/json";
        public const string SecurityToken = "X-SECURITY-TOKEN";
        public const string CST = "CST";
    }

    public class QueryParams
    {
        public const string SearchTerm = "searchTerm";
        public const string DealReference = "dealReference";
        public const string DealId = "dealId";
        public const string NodeId = "nodeId";
        public const string Limit = "limit";
        public const string Epic = "epic";
        public const string Resolution = "resolution";
        public const string Max = "max";
        public const string From = "from";
        public const string To = "to";
        public const string MarketIds = "marketIds";
        public const string WatchlistId = "watchlistId";
    }

    public class JsonFields
    {
        public const string Markets = "markets";
        public const string Status = "status";
    }

    public class Destinations
    {
        public const string SubscribeMarketData = "marketData.subscribe";
        public const string UnsubscribeMarketData = "marketData.unsubscribe";
        public const string PingService = "ping";
    }
}

