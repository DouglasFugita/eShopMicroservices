namespace Catalog.API.Entities;

public class Product
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string Category { get; private set; }
    public string Summary { get; private set; }
    public string Description { get; private set; }
    public string ImageFile { get; private set; }
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

