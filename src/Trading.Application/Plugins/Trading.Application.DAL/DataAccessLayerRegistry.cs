﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Trading.Application.BLL;

using Trading.Application.DAL.Data;
using Trading.Application.DAL.DataAccess;

namespace Trading.Application.DAL;

public static class DataAccessLayerRegistry
{
    public static void RegisterDataAccessLayerServices(this IServiceCollection services, IConfigurationRoot configuration)
    {
        ConfigurationRegistry(services, configuration);
    }

    private static void ConfigurationRegistry(IServiceCollection services, IConfiguration configuration)
    {
        //TODO: use string const
        services.AddDbContext<TradingDbContext>(options =>
            options.UseSqlite(string.Format(configuration.GetConnectionString("Sqlite"), AppDomain.CurrentDomain.BaseDirectory),
            sqliteOptionsAction: sqlOptions => sqlOptions.MigrationsAssembly("Trading.Application.DAL")));

        services.AddScoped<IPriceRepository, PriceRepository>();
    }
}
