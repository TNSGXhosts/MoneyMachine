using Microsoft.EntityFrameworkCore;

using Trading.Application.DAL.Models;

namespace Trading.Application.DAL.Data
{
    public class DataContext : DbContext
    {
        private readonly string _connectionStrign;

        public DbSet<Price> Prices { get; set; }
        public DbSet<TradingVolumes> TradingVolumes { get; set; }

        public DataContext(string connectionString)
        {
            _connectionStrign = connectionString;
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionStrign);
        }
    }
}