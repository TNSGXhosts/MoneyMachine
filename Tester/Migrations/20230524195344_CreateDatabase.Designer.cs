﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestEnv.DBEntities;

#nullable disable

namespace TestEnv.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20230524195344_CreateDatabase")]
    partial class CreateDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TestEnv.DBEntities.Price", b =>
                {
                    b.Property<int>("PriceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PriceId"));

                    b.Property<double>("ClosePriceAsk")
                        .HasColumnType("float");

                    b.Property<double>("ClosePriceBid")
                        .HasColumnType("float");

                    b.Property<double>("HighPriceAsk")
                        .HasColumnType("float");

                    b.Property<double>("HighPriceBid")
                        .HasColumnType("float");

                    b.Property<int>("LastTradedVolume")
                        .HasColumnType("int");

                    b.Property<double>("LowPriceAsk")
                        .HasColumnType("float");

                    b.Property<double>("LowPriceBid")
                        .HasColumnType("float");

                    b.Property<double>("OpenPriceAsk")
                        .HasColumnType("float");

                    b.Property<double>("OpenPriceBid")
                        .HasColumnType("float");

                    b.Property<DateTime>("SnapshotTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SnapshotTimeUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("PriceId");

                    b.ToTable("Prices");
                });
#pragma warning restore 612, 618
        }
    }
}
