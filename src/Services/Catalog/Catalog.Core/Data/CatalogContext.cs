using Catalog.Core.Data.Models;
using Catalog.Core.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;

namespace Catalog.Core.Data;

public class CatalogContext : ICatalogContext
{
    public IMongoCollection<Product> Products { get; }

    public CatalogContext(IOptions<DatabaseSettingsModel> settings)
    {
        var clientSettings = MongoClientSettings.FromConnectionString(settings.Value.ConnectionString);
        clientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());
        clientSettings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);

        var mongoClient = new MongoClient(clientSettings);

        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);

        Products = mongoDatabase.GetCollection<Product>(settings.Value.CollectionName);
        CatalogContextSeed.SeedData(Products).WaitAsync(new CancellationToken());
    }


}
