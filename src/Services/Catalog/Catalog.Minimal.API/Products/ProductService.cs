using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Minimal.API.Products;
using System.Runtime.CompilerServices;

namespace Catalog.Minimal.API.Products;

public class ProductService : IProductService
{
    private readonly ICatalogRepository _catalogRepository;

    public ProductService(ICatalogRepository catalogRepository)
    {
        _catalogRepository = catalogRepository;
    }

    public void CreateProduct(Product? product)
    {
        if (product is null) { throw new ArgumentNullException(nameof(product)); }
        _catalogRepository.CreateProduct(product);
    }

    public void DeleteProduct(string id)
    {
        _catalogRepository.DeleteProduct(id);
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _catalogRepository.GetProducts();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategory(string categoryName)
    {
        return await _catalogRepository.GetProductByCategory(categoryName);
    }

    public async Task<IEnumerable<Product>> GetProductsByName(string name)
    {
        return await _catalogRepository.GetProductByName(name);
    }

    public async Task<Product?> GetProductById(string id)
    {
        return await _catalogRepository.GetProductById(id);
    }

    public void UpdateProduct(Product? product)
    {
        if (product is null) { throw new ArgumentNullException(nameof(product)); }
        _catalogRepository.UpdateProduct(product);
    }
}
