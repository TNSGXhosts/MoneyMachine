using Microsoft.EntityFrameworkCore;

using Trading.Application.DAL.Entities;

namespace Trading.Application.DAL.Data
{
    public class TradingDbContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<PriceEntity> Prices { get; set; }
        public DbSet<TradingVolumesEntity> TradingVolumes { get; set; }

        public TradingDbContext()
        {
            // TODO : Move it to configuration or remove.
            _connectionString =
                "Filename=/Users/vitaliikonnov/Projects/MoneyMachine2/Untitled/src/Trading.Application/Trading.Application.DAL/sqlite.bd";
        }

        public TradingDbContext(string connectionString) => _connectionString = connectionString;

        public TradingDbContext(DbContextOptions<TradingDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite(_connectionString);
        }
    }
}