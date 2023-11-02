using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using MoneyMachine.Constants;
using System.Configuration;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Design;

namespace TestEnv.DBEntities
{
    partial class Context : Microsoft.EntityFrameworkCore.DbContext
    {
        public Context()
        {
        }

        //todo:think about it
        //public Context(DbContextOptions<Context> options) : base(options)
        //{
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.AppSettings.Get(ConfigurationKeys.ConnectionString));
            }
        }

        public DbSet<Price> Prices { get; set; }
    }

    [Index(nameof(Epic), nameof(Resolution), nameof(SnapshotTime), IsUnique = true, Name = "UN_Epic_Resolution_SnapshotTime")]
    public class Price
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PriceId { get; set; }
        public string Epic { get; set; }
        public string Resolution { get; set; }
        public DateTime SnapshotTime { get; set; }
        public DateTime SnapshotTimeUtc { get; set; }
        public double OpenPriceBid { get; set; }
        public double OpenPriceAsk { get; set; }
        public double ClosePriceBid { get; set; }
        public double ClosePriceAsk { get; set; }
        public double HighPriceBid { get; set; }
        public double HighPriceAsk { get; set; }
        public double LowPriceBid { get; set; }
        public double LowPriceAsk { get; set; }
        public int LastTradedVolume { get; set; }
    }
}

