﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Trading.Application.DAL.Data;

#nullable disable

namespace Trading.Application.DAL.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.13");

            modelBuilder.Entity("Trading.Application.DAL.Models.Price", b =>
                {
                    b.Property<int>("PriceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClosePriceVolumesId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("HighPriceVolumesId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("LastTradedVolume")
                        .HasColumnType("REAL");

                    b.Property<int>("LowPriceVolumesId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OpenPriceVolumesId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SnapshotTime")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SnapshotTimeUTC")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TimeFrame")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("PriceId");

                    b.HasIndex("ClosePriceVolumesId");

                    b.HasIndex("HighPriceVolumesId");

                    b.HasIndex("LowPriceVolumesId");

                    b.HasIndex("OpenPriceVolumesId");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("Trading.Application.DAL.Models.TradingVolumes", b =>
                {
                    b.Property<int>("VolumesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Ask")
                        .HasColumnType("REAL");

                    b.Property<double>("Bid")
                        .HasColumnType("REAL");

                    b.HasKey("VolumesId");

                    b.ToTable("TradingVolumes");
                });

            modelBuilder.Entity("Trading.Application.DAL.Models.Price", b =>
                {
                    b.HasOne("Trading.Application.DAL.Models.TradingVolumes", "ClosePrice")
                        .WithMany()
                        .HasForeignKey("ClosePriceVolumesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Trading.Application.DAL.Models.TradingVolumes", "HighPrice")
                        .WithMany()
                        .HasForeignKey("HighPriceVolumesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Trading.Application.DAL.Models.TradingVolumes", "LowPrice")
                        .WithMany()
                        .HasForeignKey("LowPriceVolumesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Trading.Application.DAL.Models.TradingVolumes", "OpenPrice")
                        .WithMany()
                        .HasForeignKey("OpenPriceVolumesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClosePrice");

                    b.Navigation("HighPrice");

                    b.Navigation("LowPrice");

                    b.Navigation("OpenPrice");
                });
#pragma warning restore 612, 618
        }
    }
}