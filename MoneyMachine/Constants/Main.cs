using System;
using MoneyMachine.Enums;

namespace MoneyMachine.Constants
{
    public class ApplicationConstants
    {
        public const string BaseAPIUrl = "https://demo-api-capital.backend-capital.com/api/v1";
        public const string WebSocketUrl = "wss://api-streaming-capital.backend-capital.com/connect";

        public const string DefaultDateFormat = "YYYY-MM-DDTHH:MM:SS";

        public static readonly Dictionary<Resolution, TimeSpan> ResolutionDateTimeMapping = new Dictionary<Resolution, TimeSpan>()
        {
            { Resolution.WEEK, new TimeSpan(7, 0, 0) },
            { Resolution.DAY, new TimeSpan(1, 0, 0, 0) },
            { Resolution.HOUR_4, new TimeSpan(4, 0, 0) },
            { Resolution.HOUR, new TimeSpan(1, 0, 0) },
            { Resolution.MINUTE_30, new TimeSpan(0, 30, 0) },
            { Resolution.MINUTE_15, new TimeSpan(0, 15, 0) },
            { Resolution.MINUTE_5, new TimeSpan(0, 5, 0) },
            { Resolution.MINUTE, new TimeSpan(0, 1, 0) }
        };
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
        public const string Date = "Date";
        public const string ProductPriceChanging = "Product Price Changing";
        public const string BalanceChanging = "Balance Changing";
        public const string PercentSuccessTrades = "Succes %: {0}";
    }

    public class ConfigurationKeys
    {
        public const string ApiKey = "ApiKey";
        public const string Identifier = "Identifier";
        public const string Password = "Password";
        public const string AccountName = "AccountName";
        public const string ConnectionString = "ConnectionString";
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

