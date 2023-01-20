using Catalog.API.Data.Models;
using Catalog.API.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;
using System.Security.Cryptography.X509Certificates;

namespace Catalog.API.Data;

public class CatalogContext : ICatalogContext
{
    public IMongoCollection<Product> Products { get; }

    public CatalogContext(IOptions<DatabaseSettingsModel> settings)
    {
        var clientSettings = MongoClientSettings.FromConnectionString(settings.Value.ConnectionString);
        clientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());
        var mongoClient = new MongoClient(clientSettings);

        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);

        Products = mongoDatabase.GetCollection<Product>(settings.Value.CollectionName);
        CatalogContextSeed.SeedData(Products).WaitAsync(new CancellationToken());
    }


}
