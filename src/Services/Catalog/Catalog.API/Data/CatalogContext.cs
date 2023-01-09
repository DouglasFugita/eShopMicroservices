using Catalog.API.Data.Models;
using Catalog.API.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Security.Cryptography.X509Certificates;

namespace Catalog.API.Data;

public class CatalogContext : ICatalogContext
{
    public IMongoCollection<Product> Products { get; }

    public CatalogContext(IOptions<DatabaseSettingsModel> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);

        Products = mongoDatabase.GetCollection<Product>(settings.Value.CollectionName);
        CatalogContextSeed.SeedData(Products).WaitAsync(new CancellationToken());
    }


}
