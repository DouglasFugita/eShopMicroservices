using Catalog.Core.Data;
using Catalog.Core.Data.Models;
using Catalog.Core.Repositories;

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
