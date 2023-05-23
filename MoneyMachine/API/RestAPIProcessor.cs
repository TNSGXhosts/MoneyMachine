using System;
using RestSharp;
using MoneyMachine.Constants;
using MoneyMachine.Entities;
using MoneyMachine.Extensions;
using Newtonsoft.Json;
using System.Configuration;
using System.Collections.Specialized;
using System.Text.Json;
using MoneyMachine.Enums;
using Newtonsoft.Json.Linq;
using static MoneyMachine.Tools.Serializator;
using MoneyMachine.Interface;

namespace MoneyMachine.API
{
	public class APIProcessor : IDataAccess
    {
		private RestClient _client;

        //Valid for 10 minuts after the last use
        private string _CST = null;
        private string _securityToken = null;
        private string _accountName;

        public APIProcessor(RestClient restClient)
		{
			_client = restClient;
		}

		public APIProcessor()
		{
            _client = new RestClient(ApplicationConstants.BaseAPIUrl);
            StartSession();
            _accountName = ConfigurationManager.AppSettings.Get("AccountName");
            if (!string.IsNullOrEmpty(_accountName))
                throw new Exception("Cannot get account Name");
        }

        public void StartSession()
        {
            var apiKey = ConfigurationManager.AppSettings.Get("ApiKey");
            var identifier = ConfigurationManager.AppSettings.Get("Identifier");
            var password = ConfigurationManager.AppSettings.Get("Password");
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(identifier) || string.IsNullOrEmpty(password))
                throw new Exception("Cannot get apiKey, Identifier, Password");
            var request = new RestRequest(EndPoints.Session, Method.Post);
            request.AddHeader(Constants.Headers.APIKey, apiKey);
            request.AddHeader(Constants.Headers.ContentType, Headers.AppJson);
            request.AddParameter(Headers.AppJson, new SessionConnectData()
            {
                Identifier = identifier,
                Password = password
            }, ParameterType.RequestBody);

            var response = _client.Execute(request);
            response.ProcessResponse();

            var cst = response?.Headers?.FirstOrDefault(h => h.Name == Headers.CST);
            var securityToken = response?.Headers?.FirstOrDefault(h => h.Name == Headers.SecurityToken);
            if (cst != null && securityToken != null && cst.Value != null && securityToken.Value != null)
            {
                _CST = (string)cst.Value;
                _securityToken = (string)securityToken.Value;
            }
            else
                throw new Exception($"Session doesn't start. {cst} - {securityToken}");

            Console.WriteLine("Session successful started.");
        }

        #region Session

        public List<Account> GetAccounts()
        {
            var request = new RestRequest(EndPoints.Accounts, Method.Get);
            request.SetCommonRequestData(_securityToken, _CST);
            var result = _client.Execute(request);
            result.ProcessResponse();
            AccountsEntity accounts = DeserializeObject<AccountsEntity>(result?.Content);
            return accounts.Accounts;
        }

        public double GetBalance()
        {
            var accounts = GetAccounts();
            if (accounts == null || !accounts.Any())
                throw new Exception("Wrong accounts data");

            return accounts.First(a => a.AccountName == _accountName).Balance.Available;
        }

        public SessionInfo GetSessionInfo()
        {
            var request = new RestRequest(EndPoints.Session, Method.Get);
            request.SetCommonRequestData(_securityToken, _CST);
            var result = _client.Execute(request);
            result.ProcessResponse();
            SessionInfo sessionInfo = JsonConvert.DeserializeObject<SessionInfo>(result.Content);
            if (sessionInfo == null)
                throw new Exception($"Wrong Get Session Response: {result.Content}");

            return sessionInfo;
        }

        public bool LogoutSession()
        {
            var request = new RestRequest(EndPoints.Session, Method.Delete);
            request.SetCommonRequestData(_securityToken, _CST);
            var result = _client.Execute(request);
            var status = DeserializeObject<StatusEntity>(result?.Content);
            Console.WriteLine($"Session logout {status.Status}");
            return status.Status.Equals(Status.SUCCESS);
        }

        #endregion

        #region Trading

        //Position/Order confirmation
        public Deal GetDeal(string dealReference)
        {
            var request = new RestRequest(EndPoints.Confirms, Method.Get);
            request.AddQueryParameter(QueryParams.DealReference, dealReference);
            request.SetCommonRequestData(_securityToken, _CST);
            var result = _client.Execute(request);
            result.ProcessResponse();
            var deal = result?.Content != null ? JsonConvert.DeserializeObject<Deal>(result.Content) : null;
            if (deal == null)
                throw new Exception($"Wrong deal data: {result?.Content}");

            return deal;
        }

        #endregion

        #region Positions

        //All positions
        public List<OpenPosition> GetAllPositions()
        {
            var request = new RestRequest(EndPoints.Positions, Method.Get);
            request.SetCommonRequestData(_securityToken, _CST);
            var result = _client.Execute(request);
            result.ProcessResponse();
            var positions = JsonConvert.DeserializeObject<PositionJsonEntity>(result?.Content);
            if (positions == null)
                throw new Exception($"Wrong Get All Positions Response: {result?.Content}");

            return positions.positions;
        }

        //Single position
        public OpenPosition GetPosition(string dealId)
        {
            var request = new RestRequest(EndPoints.Positions, Method.Get);
            request.SetCommonRequestData(_securityToken, _CST);
            request.AddQueryParameter(QueryParams.DealId, dealId);
            var result = _client.Execute(request);
            result.ProcessResponse();
            var position = DeserializeObject<OpenPosition>(result.Content);
            return position;
        }

        //Update position
        public string UpdatePosition(PositionUpdateEntity position, string dealId)
        {
            var request = new RestRequest(EndPoints.Positions, Method.Put);
            request.SetCommonRequestData(_securityToken, _CST);
            request.AddQueryParameter(QueryParams.DealId, dealId);
            request.AddHeader(Constants.Headers.ContentType, Headers.AppJson);
            var jsonString = SerializeObject<PositionUpdateEntity>(position);
            request.AddParameter(Headers.AppJson, jsonString, ParameterType.RequestBody);
            var result = _client.Execute(request);
            result.ProcessResponse();
            var dealRef = DeserializeObject<DealRef>(result?.Content);
            return dealRef.DealReference;
        }

        //Create position
        public string CreatePosition(PositionCreateEntity positionSetup)
        {
            var request = new RestRequest(EndPoints.Positions, Method.Post);
            request.SetCommonRequestData(_securityToken, _CST);
            request.AddHeader(Constants.Headers.ContentType, Headers.AppJson);
            var jsonString = SerializeObject<PositionCreateEntity>(positionSetup);
            request.AddParameter(Headers.AppJson, jsonString, ParameterType.RequestBody);
            var result = _client.Execute(request);
            result.ProcessResponse();
            var dealReference = JsonConvert.DeserializeObject<DealRef>(result?.Content);
            if (dealReference == null || string.IsNullOrEmpty(dealReference.DealReference))
                throw new Exception($"Wrong Deal Reference {result?.Content}");

            return dealReference.DealReference;
        }

        //Close position
        public string ClosePosition(string dealId)
        {
            var request = new RestRequest(EndPoints.Positions, Method.Delete);
            request.SetCommonRequestData(_securityToken, _CST);
            request.AddQueryParameter(QueryParams.DealId, dealId);
            var result = _client.Execute(request);
            result.ProcessResponse();
            var dealReference = DeserializeObject<DealRef>(result?.Content);
            return dealReference.DealReference;
        }

        #endregion

        #region Working Orders

        //All working orders
        public List<WorkingOrder> GetWorkingOrders()
        {
            var request = new RestRequest(EndPoints.WorkingOrders, Method.Get);
            request.SetCommonRequestData(_securityToken, _CST);
            var result = _client.Execute(request);
            result.ProcessResponse();
            var workingOrders = DeserializeObject<WorkingOrders>(result?.Content);
            return workingOrders.WorkingOrderList;
        }

        public string SyncWorkingOrder<T>(T workingOrder, Method method)
        {
            if (nameof(T) != nameof(WorkingOrderCreateEntity) || nameof(T) != nameof(WorkingOrderUpdateEntity))
                throw new Exception($"Wrong input sync data: {workingOrder}");

            var request = new RestRequest(EndPoints.WorkingOrders, method);
            request.SetCommonRequestData(_securityToken, _CST);
            request.AddHeader(Headers.ContentType, Headers.AppJson);
            var jsonString = SerializeObject<T>(workingOrder);
            request.AddParameter(Headers.AppJson, jsonString, ParameterType.RequestBody);
            var result = _client.Execute(request);
            var dealReference = DeserializeObject<DealRef>(result?.Content);
            return dealReference.DealReference;
        }

        public string DeleteWorkingOrder(string dealId)
        {
            var request = new RestRequest(EndPoints.WorkingOrders, Method.Delete);
            request.SetCommonRequestData(_securityToken, _CST);
            request.AddQueryParameter(QueryParams.DealId, dealId);
            var result = _client.Execute(request);
            result.ProcessResponse();
            var dealReference = DeserializeObject<DealRef>(result?.Content);
            return dealReference.DealReference;
        }

        #endregion

        #region Markets

        public List<Node> GetTopLevelMarketCategories()
        {
            return GetMarketNodes();
        }

        public List<Node> GetAllCategorySubNodes(string nodeId, int? limit = null)
        {
            return GetMarketNodes(nodeId, limit);
        }

        private List<Node> GetMarketNodes(string nodeId = null, int? limit = null)
        {
            var request = new RestRequest(EndPoints.MarketNavigation, Method.Get);
            request.SetCommonRequestData(_securityToken, _CST);
            if (!string.IsNullOrEmpty(nodeId))
                request.AddQueryParameter(QueryParams.NodeId, nodeId);
            if (limit != null)
                request.AddQueryParameter(QueryParams.Limit, limit.ToString());
            var result = _client.Execute(request);
            var nodes = DeserializeObject<Nodes>(result?.Content);
            return nodes.NodeList;
        }

        //Market details
        public List<MarketDetail> GetMarkets(string searchTerm)
        {
            var request = new RestRequest(EndPoints.Markets, Method.Get);
            request.AddQueryParameter(QueryParams.SearchTerm, searchTerm);
            request.SetCommonRequestData(_securityToken, _CST);
            var result = _client.Execute(request);
            result.ProcessResponse();
            var marketResponse = DeserializeObject<MarketResponseEntity>(result?.Content);
            return marketResponse.Markets;
        }

        public MarketDetails GetSingleMarketDetails(string epic)
        {
            var request = new RestRequest($"{EndPoints.Markets}/{epic}", Method.Get);
            request.SetCommonRequestData(_securityToken, _CST);
            var result = _client.Execute(request);
            result.ProcessResponse();
            var marketDetails = DeserializeObject<MarketDetails>(result?.Content);
            return marketDetails;
        }

        #endregion

        #region Prices

        public PricesEntity GetHistoricalPrices(string epic, Resolution? resolution = null, int? max = null, DateTime? from = null, DateTime? to = null)
        {
            var request = new RestRequest($"{EndPoints.Prices}/{epic}", Method.Get);
            request.SetCommonRequestData(_securityToken, _CST);
            if (resolution != null)
                request.AddQueryParameter(QueryParams.Resolution, resolution.ToString());
            if (max.HasValue)
                request.AddQueryParameter(QueryParams.Max, max.ToString());
            if (from.HasValue)
                request.AddQueryParameter(QueryParams.From, from.Value.ToUniversalTime().ToString("s"));
            if (to.HasValue)
                request.AddQueryParameter(QueryParams.To, to.Value.ToUniversalTime().ToString("s"));
            var result = _client.Execute(request);
            result.ProcessResponse();
            var prices = DeserializeObject<PricesEntity>(result?.Content);
            return prices;
        }

        public PricesEntity GetHistoricalPricesOld(string epic, Resolution resolution, int max, DateTime from, DateTime to)
        {
            var request = new RestRequest($"{EndPoints.Prices}/{epic}?resolution={resolution.ToString()}&max={max.ToString()}&from={from.ToUniversalTime().ToString("s")}&to={to.ToUniversalTime().ToString("s")}", Method.Get);
            request.SetCommonRequestData(_securityToken, _CST);
            var result = _client.Execute(request);
            result.ProcessResponse();
            var prices = DeserializeObject<PricesEntity>(result?.Content);
            return prices;
        }

        #endregion

        #region Client Sentiment

        public List<ClientSentiment> GetClientSentiments(List<string> marketIds)
        {
            var request = new RestRequest(EndPoints.ClientSentiment, Method.Get);
            request.SetCommonRequestData(_securityToken, _CST);
            if (marketIds != null && marketIds.Any())
            {
                var paramString = string.Join(",", marketIds);
                request.AddQueryParameter(QueryParams.MarketIds, paramString);
            }
            var result = _client.Execute(request);
            result.ProcessResponse();
            var clientSentiments = DeserializeObject<ClientSentiments>(result?.Content);
            return clientSentiments.ClientSentimentList;
        }

        public ClientSentiment GetClientSentiment(string marketId)
        {
            if (string.IsNullOrEmpty(marketId))
                throw new Exception("MarketId is empty");
            var request = new RestRequest($"{EndPoints.ClientSentiment}/{marketId}", Method.Get);
            request.SetCommonRequestData(_securityToken, _CST);
            var result = _client.Execute(request);
            result.ProcessResponse();
            var clientSentiment = DeserializeObject<ClientSentiment>(result?.Content);
            return clientSentiment;
        }

        #endregion

        #region WatchList

        public List<Watchlist> GetWatchlists()
        {
            var request = new RestRequest(EndPoints.Watchlists, Method.Get);
            request.SetCommonRequestData(_securityToken, _CST);
            var result = _client.Execute(request);
            result.ProcessResponse();
            var watchlists = DeserializeObject<Watchlists>(result?.Content);
            return watchlists.WatchlistList;
        }

        public WatchlistResultEntity CreateWatchList(WatchlistCreateEntity watchlist)
        {
            var request = new RestRequest(EndPoints.Watchlists, Method.Post);
            request.SetCommonRequestData(_securityToken, _CST);
            request.AddHeader(Headers.ContentType, Headers.AppJson);
            var jsonString = SerializeObject<WatchlistCreateEntity>(watchlist);
            request.AddParameter(Headers.AppJson, jsonString, ParameterType.RequestBody);
            var result = _client.Execute(request);
            var operationResult = DeserializeObject<WatchlistResultEntity>(result?.Content);
            Console.WriteLine($"Create watch list operation: {operationResult.Status}");
            return operationResult;
        }

        public List<Market> GetWatchlist(string watchlistId)
        {
            var request = new RestRequest($"{EndPoints.Watchlists}/{watchlistId}", Method.Get);
            request.SetCommonRequestData(_securityToken, _CST);
            var result = _client.Execute(request);
            result.ProcessResponse();
            var markets = DeserializeListFieldByName<Market>(result?.Content, JsonFields.Markets);
            return markets;
        }

        public string AddMarketToWatchlist(string watchlistId, EpicNameEntity epic)
        {
            var request = new RestRequest($"{EndPoints.Watchlists}/{watchlistId}", Method.Put);
            request.SetCommonRequestData(_securityToken, _CST);
            var jsonString = SerializeObject<EpicNameEntity>(epic);
            request.AddHeader(Headers.ContentType, Headers.AppJson);
            request.AddParameter(Headers.AppJson, jsonString, ParameterType.RequestBody);
            var result = _client.Execute(request);
            result.ProcessResponse();
            var operationResult = DeserializeFieldByName<string>(result?.Content, JsonFields.Status);
            Console.WriteLine($"Add market to watchlist: {operationResult}");
            return operationResult;
        }

        public string DeleteWatchlist(string watchlistId)
        {
            var request = new RestRequest($"{EndPoints.Watchlists}/{watchlistId}", Method.Delete);
            request.SetCommonRequestData(_securityToken, _CST);
            var result = _client.Execute(request);
            result.ProcessResponse();
            var operationStatus = DeserializeFieldByName<string>(result?.Content, JsonFields.Status);
            Console.WriteLine($"Delete watchlist: {operationStatus}");
            return operationStatus;
        }

        public string DeleteMarketFromWatchlist(string watchlistId, string epic)
        {
            var request = new RestRequest($"{EndPoints.Watchlists}/{QueryParams.WatchlistId}/{QueryParams.Epic}", Method.Delete);
            request.SetCommonRequestData(_securityToken, _CST);
            var result = _client.Execute(request);
            var operationStatus = DeserializeFieldByName<string>(result?.Content, JsonFields.Status);
            Console.WriteLine($"Delete market from watchlist: {operationStatus}");
            return operationStatus;
        }

        #endregion

        #region Tools

        public Tuple<string, string> GetTockens()
        {
            return new Tuple<string, string>(_securityToken, _CST);
        }

        public PricesEntity GetHistoricalPricesHourPeriodForTesting(string epic, Resolution? resolution = null, int? max = null, DateTime? from = null, DateTime? to = null)
        {
            throw new NotImplementedException("Testing metod");
        }

        #endregion
    }
}

