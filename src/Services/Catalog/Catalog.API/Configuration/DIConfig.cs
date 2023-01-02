using Catalog.API.Repositories;

namespace Catalog.API.Configuration;

public static class DIConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ICatalogRepository, CatalogRepository>();
    }
}
