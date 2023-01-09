using Catalog.API.Data;
using Catalog.API.Data.Models;
using Catalog.API.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.API.Configuration;

public static class DIConfig
{
    public static void RegisterServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<DatabaseSettingsModel>(configuration.GetSection("DatabaseSettings"));
        services.AddSingleton<ICatalogContext, CatalogContext>();
        services.AddScoped<ICatalogRepository, CatalogRepository>();

    }
}
