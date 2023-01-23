using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Core.Data;

public interface ICatalogContext
{
    IMongoCollection<Product> Products { get;}
}
