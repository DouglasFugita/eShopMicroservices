using Catalog.Core.Data;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Moq;

namespace Catalog.UnitTests.Fixtures;
public class CatalogContextFixture
{
    public Mock<ICatalogContext> MockContext { get; }
    public Mock<IMongoCollection<Product>> MockProductCollection { get;}
    public Mock<IAsyncCursor<Product>> MockCursor { get; }
    public Product ExampleProduct { get; }
    public CatalogRepository CatalogRepository { get; }
    public string FilterParam { get; }
    public string FilterIdParam { get; }


    public CatalogContextFixture()
    {
        MockCursor = new Mock<IAsyncCursor<Product>>();
        MockCursor.Setup(x => x.Current).Returns(new List<Product>());

        MockProductCollection = new Mock<IMongoCollection<Product>>();

        MockContext = new Mock<ICatalogContext>();
        MockContext.Setup(x => x.Products).Returns(MockProductCollection.Object);

        CatalogRepository = new CatalogRepository(MockContext.Object);

        FilterParam = "string";
        FilterIdParam = "602d2149e773f2a3990b47fa";
        ExampleProduct = new Product("id", "name", "category", "summary", "description", "imageFile", 100);
    }

    public static BsonDocument FilterRender(FilterDefinition<Product> filter)
    {
        var serializerRegistry = BsonSerializer.SerializerRegistry;
        var documentSerializer = serializerRegistry.GetSerializer<Product>();
        return filter.Render(documentSerializer, serializerRegistry);
    }
}
