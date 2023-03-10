using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Minimal.API.Products;
using Catalog.UnitTests.Fixtures;
using Common.Caching;
using Microsoft.AspNetCore.Builder;
using Moq;
using RabbitMQ.Client;
using Serilog;

namespace Catalog.UnitTests.MinimalAPI;

public class ProductServiceTests
{
    [Fact]
    public void CreateProduct_ShoulCall()
    {
        //Arrange
        var mockCatalogRepository = new Mock<ICatalogRepository>();
        var mockRedis = new Mock<IRedisCacheProvider>();
        var mockConnectionFactory = new Mock<IConnectionFactory>();
        var mockLogger = new Mock<ILogger>();
        
        var product = ProductsFixtures.GetFakeProduct.Generate(1).First();

        var productService = new ProductService(mockCatalogRepository.Object, mockRedis.Object, mockLogger.Object,
            mockConnectionFactory.Object);
        
        //Act
        productService.CreateProduct(product);

        //Assert
        mockCatalogRepository.Verify(x => x.CreateProduct(It.IsAny<Product>()), Times.Once);

    }
  
}