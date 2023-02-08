using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Serilog;

namespace Catalog.Minimal.API.Products;

public class ProductService : IProductService
{
    private readonly ICatalogRepository _catalogRepository;
    private readonly IDistributedCache _cache;
    private readonly Serilog.ILogger _logger;
    public ProductService(ICatalogRepository catalogRepository, IDistributedCache cache, Serilog.ILogger logger)
    {
        _catalogRepository = catalogRepository;
        _cache = cache;
        _logger = logger;
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
        var cacheProduct = _cache.GetString(id);
        if (cacheProduct != null)
        {
            _logger.Information("Product {id} get from Caching", id);
            return JsonSerializer.Deserialize<Product>(cacheProduct);
        }
        var product = await _catalogRepository.GetProductById(id);
        if (product != null)
        {
            var timeout = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3),
                SlidingExpiration = TimeSpan.FromMinutes(1),
            };

            _logger.Information("Product {id} get from DB", id);
            _cache.SetString(id, JsonSerializer.Serialize(product), timeout);
        }
        return product;
    }

    public void UpdateProduct(Product? product)
    {
        if (product is null) { throw new ArgumentNullException(nameof(product)); }
        _catalogRepository.UpdateProduct(product);
    }
}
