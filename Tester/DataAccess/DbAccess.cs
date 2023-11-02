using System;
using System.Diagnostics;
using TestEnv.DBEntities;
using MoneyMachine.Interface;

namespace TestEnv.DataAccess
{
    public class DbAccess
    {
        private ILogger Logger;

        public DbAccess(ILogger logger)
        {
            Logger = logger;
        }

        public void SavePrices(List<Price> prices)
        {
            if (prices != null && prices.Count > 0)
                using (var db = new Context())
                {
                    prices.ForEach(p => db.Prices.Add(p));
                    db.SaveChanges();
                    Logger.Log($"Prices Saved ({prices.First().Epic}, {prices.First().SnapshotTime}-{prices.Last().SnapshotTime})", 0);
                }
        }

        public List<Price> GetPrices(string epic, string resolution, DateTime from, DateTime to)
        {
            var prices = new List<Price>();
            if (!string.IsNullOrEmpty(epic) && !string.IsNullOrEmpty(resolution))
                using (var db = new Context())
                {
                    prices = db.Prices.Where(p => p.Epic.Equals(epic, StringComparison.OrdinalIgnoreCase)
                    && p.Resolution.Equals(resolution, StringComparison.OrdinalIgnoreCase)
                    && p.SnapshotTime >= from && p.SnapshotTime <= to).ToList();
                }

            return prices;
        }

        public bool ValidateIsPriceAlreadyExists(string epic, string resolution, DateTime snapshotTime)
        {
            using (var db = new Context())
            {
                return db.Prices.Any(p => p.Epic.Equals(epic, StringComparison.OrdinalIgnoreCase)
                && p.Resolution.Equals(resolution, StringComparison.OrdinalIgnoreCase)
                && p.SnapshotTime == snapshotTime);
            }
        }
    }
}


