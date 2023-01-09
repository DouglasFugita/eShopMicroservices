using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Entities;

public class Product
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; private set; }
    [BsonElement("Name")]
    public string Name { get; private set; }
    [BsonElement("Category")]
    public string Category { get; private set; }
    [BsonElement("Summary")]
    public string Summary { get; private set; }
    [BsonElement("Description")]
    public string Description { get; private set; }
    [BsonElement("ImageFile")]
    public string ImageFile { get; private set; }
    [BsonElement("Price")]
    public Decimal Price { get; private set; }

    public Product(string id, string name, string category, string summary, string description, string imageFile, decimal price)
    {
        Id = id;
        Name = name;
        Category = category;
        Summary = summary;
        Description = description;
        ImageFile = imageFile;
        Price = price;
    }

}

