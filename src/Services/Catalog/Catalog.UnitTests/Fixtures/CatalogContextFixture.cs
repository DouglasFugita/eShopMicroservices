using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.UnitTests.Fixtures;
public class CatalogContextFixture
{
    public Mock<ICatalogContext> MockContext { get; }
    public Mock<IMongoCollection<Product>> MockProductCollection { get;}
    public Mock<IAsyncCursor<Product>> MockCursor { get; }
    public Product ExampleProduct { get; }
    public CatalogRepository CatalogRepository { get; }
    public string FilterParam { get; }


    public CatalogContextFixture()
    {
        MockCursor = new Mock<IAsyncCursor<Product>>();
        MockCursor.Setup(x => x.Current).Returns(new List<Product>());

        MockProductCollection = new Mock<IMongoCollection<Product>>();

        MockContext = new Mock<ICatalogContext>();
        MockContext.Setup(x => x.Products).Returns(MockProductCollection.Object);

        CatalogRepository = new CatalogRepository(MockContext.Object);

        FilterParam = "string";
        ExampleProduct = new Product("id", "name", "category", "summary", "description", "imageFile", 100);
    }

    public BsonDocument FilterRender(FilterDefinition<Product> filter)
    {
        var serializerRegistry = BsonSerializer.SerializerRegistry;
        var documentSerializer = serializerRegistry.GetSerializer<Product>();
        return filter.Render(documentSerializer, serializerRegistry);
    }
}
