using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Core.Data;

public static class CatalogContextSeed
{
    public static async Task SeedData(IMongoCollection<Product> productCollection)
    {
        var existProduct = await productCollection.CountDocumentsAsync(p => true) > 0;
        if (!existProduct)
        {
            await productCollection.InsertManyAsync(GetPreconfiguredProducts());
        }
    }

    public static IEnumerable<Product> GetPreconfiguredProducts()
    {
        var summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.";
        var description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.";

        return new List<Product>
        {
            new Product(id: "602d2149e773f2a3990b47f5", name: "IPhone X", category: "Smart Phone", summary: summary, description: description,imageFile: "product-1.png",price: 950.00M),
            new Product(id: "602d2149e773f2a3990b47f6", name: "Samsung 10", category: "Smart Phone", summary: summary, description: description, imageFile: "product-2.png",price: 840.00M),
            new Product(id: "602d2149e773f2a3990b47f7",name: "Huawei Plus",category: "White Appliances",summary: summary,description: description,imageFile: "product-3.png",price: 650.00M),
            new Product(id: "602d2149e773f2a3990b47f8",name: "Xiaomi Mi 9",category: "White Appliances",summary: summary,description: description,imageFile: "product-4.png",price: 470.00M),
            new Product(id: "602d2149e773f2a3990b47f9",name: "HTC U11+ Plus",category: "Smart Phone",summary: summary,description: description,imageFile: "product-5.png",price: 380.00M),
            new Product(id: "602d2149e773f2a3990b47fa",name: "LG G7 ThinQ",category: "Home Kitchen",summary: summary,description: description,imageFile: "product-6.png",price: 240.00M)
        };
    }
}