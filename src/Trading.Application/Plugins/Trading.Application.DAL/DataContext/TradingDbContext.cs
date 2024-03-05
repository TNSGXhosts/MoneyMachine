using Microsoft.EntityFrameworkCore;

using Core.Models;
using Core;

namespace Trading.Application.DAL.Data;

public class TradingDbContext : DbContext
{
    //TODO: use string const
    private readonly string _connectionString
#pragma warning disable RCS0056 // A line is too long.
        = "Data Source=/Users/vitaliikonnov/Projects/MoneyMachineNet8/src/Trading.Application/Plugins/Trading.Application.DAL/sqlite.sqlite;";
#pragma warning restore RCS0056 // A line is too long.

    public DbSet<PriceEntity> Prices { get; set; }
    public DbSet<TradingVolumesEntity> TradingVolumes { get; set; }
    public DbSet<PriceBatch> PriceBatches { get; set; }

    public TradingDbContext() { }

    public TradingDbContext(DbContextOptions<TradingDbContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PriceEntity>(entity => entity.HasKey(k => k.PriceId));
        modelBuilder.Entity<TradingVolumesEntity>(entity => entity.HasKey(k => k.VolumesId));
        modelBuilder.Entity<PriceBatch>(batch => {
            batch.HasKey(k => k.PriceBatchId);
            batch.HasIndex(e => new { e.Ticker, e.TimeFrame, e.Period });
        });
    }
}