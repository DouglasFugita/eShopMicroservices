using Catalog.Core.Entities;

namespace Catalog.Minimal.API.Products;

public interface IProductService
{
    void CreateProduct(Product? product);
    void UpdateProduct(Product? product);
    void DeleteProduct(string id);

    Task<IEnumerable<Product>> GetProducts();
    Task<Product?> GetProductById(string id);
    Task<IEnumerable<Product>> GetProductsByName(string name);
    Task<IEnumerable<Product>> GetProductsByCategory(string categoryName);
}