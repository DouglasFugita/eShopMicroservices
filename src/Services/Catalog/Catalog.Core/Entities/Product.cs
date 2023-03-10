using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Core.Entities;

public class Product
{
    public Product(string id, string name, string category, string summary, string description, string imageFile,
        decimal price)
    {
        Id = id;
        Name = name;
        Category = category;
        Summary = summary;
        Description = description;
        ImageFile = imageFile;
        Price = price;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; }

    [BsonElement("Name")] public string Name { get;}

    [BsonElement("Category")] public string Category { get; }

    [BsonElement("Summary")] public string Summary { get; }

    [BsonElement("Description")] public string Description { get; }

    [BsonElement("ImageFile")] public string ImageFile { get; }

    [BsonElement("Price")] public decimal Price { get; }

    public void AddNewId(string id)
    {
        this.Id = id;
    }
}