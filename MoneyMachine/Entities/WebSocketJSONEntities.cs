using System;
namespace MoneyMachine.Entities
{
    public class MarketDataSubscriber
    {
        public string Destination { get; set; }
        public string CorrelationId { get; set; }
        public string Cst { get; set; }
        public string SecurityToken { get; set; }
        public PayLoad Payload { get; set; }
    }

    public class PayLoad
    {
        public List<string> Epics { get; set; }
    }

    public class MarketDataUpdateEntity
    {
        public string Status { get; set; }
        public string Destination { get; set; }
        public MarketUpdate Payload { get; set; } 
    }

    public class MarketUpdate
    {
        public string Epic { get; set; }
        public string Product { get; set; }
        public double Bid { get; set; }
        public double BidQty { get; set; }
        public double Ofr { get; set; }
        public double OfrQty { get; set; }
        public long timestamp { get; set; }
    }

    public class PingEntity
    {
        public string Destination { get; set; }
        public string CorrelationId { get; set; }
        public string Cst { get; set; }
        public string SecurityToken { get; set; }
    }

    public class PingResponseEntity
    {
        public string Status { get; set; }
        public string Destination { get; set; }
        public string CorrelationId { get; set; }
    }
}

