using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Core.Data;

public static class CatalogContextSeed
{
    public static async Task SeedData(IMongoCollection<Product> productCollection)
    {
        var existProduct = (productCollection.CountDocuments(p => true));
        if (existProduct < 1) await productCollection.InsertManyAsync(GetPreconfiguredProducts());
    }

    public static IEnumerable<Product> GetPreconfiguredProducts()
    {
        const string summary =
            "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.";
        const string description =
            "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.";

        return new List<Product>
        {
            new("602d2149e773f2a3990b47f5", "IPhone X", "Smart Phone", summary, description, "product-1.png", 950.00M),
            new("602d2149e773f2a3990b47f6", "Samsung 10", "Smart Phone", summary, description, "product-2.png",
                840.00M),
            new("602d2149e773f2a3990b47f7", "Huawei Plus", "White Appliances", summary, description, "product-3.png",
                650.00M),
            new("602d2149e773f2a3990b47f8", "Xiaomi Mi 9", "White Appliances", summary, description, "product-4.png",
                470.00M),
            new("602d2149e773f2a3990b47f9", "HTC U11+ Plus", "Smart Phone", summary, description, "product-5.png",
                380.00M),
            new("602d2149e773f2a3990b47fa", "LG G7 ThinQ", "Home Kitchen", summary, description, "product-6.png",
                240.00M)
        };
    }
}