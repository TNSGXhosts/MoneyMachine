using System;
using System.Threading;
using Websocket.Client;
using Newtonsoft.Json;
using MoneyMachine.Constants;
using System.Security.Policy;
using MoneyMachine.Entities;
using static MoneyMachine.Tools.Serializator;

namespace MoneyMachine.API
{
	public class WebSocketClient : IDisposable
	{
        private WebsocketClient _client;
        private string _securityTocken;
        private string _CST;

        public WebSocketClient(Tuple<string, string> data)
		{
            var url = new Uri(ApplicationConstants.WebSocketUrl);
            _client = new WebsocketClient(url);
            _securityTocken = data.Item1;
            _CST = data.Item2;
        }

		public async Task Start(string securityTocken, string cst)
		{
            var url = new Uri(ApplicationConstants.WebSocketUrl);
            _client = new WebsocketClient(url);
            _securityTocken = securityTocken;
            _CST = cst;
        }

		public void SubscribeMarketData(List<string> epics)
		{
            try
            {
                var i = 0;
                var exitEvent = new ManualResetEvent(false);

                _client.ReconnectTimeout = TimeSpan.FromSeconds(30);
                _client.ReconnectionHappened.Subscribe(info =>
                {
                    Console.WriteLine("Reconnection happened, type: " + info.Type);
                    i = i++;
                    if (i == 5)
                        UnsubscribeMarketData(new List<string>() { "BTCUSD" });
                });
                _client.MessageReceived.Subscribe(m => Console.WriteLine(m));
                _client.Start();
                var subscriber = new MarketDataSubscriber()
                {
                    Destination = Destinations.SubscribeMarketData,
                    CorrelationId = 1.ToString(),
                    Cst = _CST,
                    SecurityToken = _securityTocken,
                    Payload = new PayLoad() { Epics = epics }
                };
                var jsonString = SerializeObject<MarketDataSubscriber>(subscriber);
                Task.Run(() => _client.Send(jsonString));
                exitEvent.WaitOne();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.ToString());
            }
        }

        public void UnsubscribeMarketData(List<string> epics)
        {
            try
            {
                var exitEvent = new ManualResetEvent(false);

                _client.ReconnectTimeout = TimeSpan.FromSeconds(30);
                _client.ReconnectionHappened.Subscribe(info =>
                {
                    Console.WriteLine("Reconnection happened, type: " + info.Type);
                });
                _client.MessageReceived.Subscribe(m => {
                    Console.WriteLine(m);
                    });
                _client.Start();
                var subscriber = new MarketDataSubscriber()
                {
                    Destination = Destinations.UnsubscribeMarketData,
                    CorrelationId = 2.ToString(),
                    Cst = _CST,
                    SecurityToken = _securityTocken,
                    Payload = new PayLoad() { Epics = epics }
                };
                var jsonString = SerializeObject<MarketDataSubscriber>(subscriber);
                Task.Run(() => _client.Send(jsonString));
                exitEvent.WaitOne();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.ToString());
            }
        }


        public void Dispose()
        {
            _client.Dispose();
        }
    }
}

