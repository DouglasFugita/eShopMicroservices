using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Common.Caching;
using RabbitMQ.Client;
using System.Text.Json;

namespace Catalog.Minimal.API.Products;

public class ProductService : IProductService
{
    private readonly ICatalogRepository _catalogRepository;
    private readonly Serilog.ILogger _logger;
    private readonly IRedisCacheProvider _cache;

    private readonly IConnectionFactory _connectionFactory;
    public ProductService(ICatalogRepository catalogRepository, IRedisCacheProvider cache, Serilog.ILogger logger, IConnectionFactory connectionFactory)
    {
        _catalogRepository = catalogRepository;
        _cache = cache;
        _logger = logger;
        _connectionFactory = connectionFactory;
    }

    public void CreateProduct(Product? product)
    {
        if (product is null) { throw new ArgumentNullException(nameof(product)); }

        _catalogRepository.CreateProduct(product);
        _cache.Set<Product>(product.Id, product);


        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        var exchange = "catalog_exchange";
        var queue = "catalog_queue";
        var routingKey = "catalog_routingkey";

        channel.ExchangeDeclare(exchange, ExchangeType.Direct);
        channel.QueueDeclare(queue, true, false, false, null);
        channel.QueueBind(queue, exchange, routingKey);

        var body = System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(product));

        channel.BasicPublish(exchange, routingKey, null, body);
    }

    public void DeleteProduct(string id)
    {
        _catalogRepository.DeleteProduct(id);
        _cache.Remove<Product>(id);
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
        var cacheProduct = _cache.Get<Product>(id);
        if (cacheProduct != null)
        {
            _logger.Information("Product {id} get from Caching", id);
            return cacheProduct;
        }
        var product = await _catalogRepository.GetProductById(id);
        if (product != null)
        {
            _logger.Information("Product {id} get from DB", id);
            _cache.Set<Product>(id, product);
        }
        return product;
    }

    public void UpdateProduct(Product? product)
    {
        if (product is null) { throw new ArgumentNullException(nameof(product)); }
        _catalogRepository.UpdateProduct(product);
        _cache.Set<Product>(product.Id, product);
    }
}
