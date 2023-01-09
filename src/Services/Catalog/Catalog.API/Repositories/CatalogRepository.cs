using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public class CatalogRepository: ICatalogRepository
{
    private readonly ICatalogContext _context;
    private readonly IMongoCollection<Product> ProductsCollection;

    public CatalogRepository(ICatalogContext context)
    {
        _context = context;
        ProductsCollection = _context.Products;
    }

    public async Task CreateProduct(Product product)
    {
        await ProductsCollection.InsertOneAsync(product);
    }

    public async Task<bool> DeleteProduct(string id)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        DeleteResult deleteResult = await ProductsCollection.DeleteOneAsync(filter);

        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }

    public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);
        return await ProductsCollection.Find(filter).ToListAsync();
    }

    public async Task<Product?> GetProductById(string id)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        return await ProductsCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
        return await ProductsCollection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProducts()
    { 
        return await ProductsCollection.Find(p => true).ToListAsync();
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var updateResult = await ProductsCollection.ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);

        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }
}
