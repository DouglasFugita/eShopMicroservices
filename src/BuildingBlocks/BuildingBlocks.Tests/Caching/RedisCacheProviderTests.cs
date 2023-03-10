using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Bogus;
using Catalog.Core.Entities;
using Common.Caching;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace BuildingBlocks.Tests.Caching;
public class RedisCacheProviderTests
{
    public static Faker<Product> GetFakeProduct { get; } =
        new Faker<Product>()
            .CustomInstantiator(f =>
                new Product(
                    f.Commerce.Ean8(),
                    f.Commerce.ProductName(),
                    f.Commerce.Department(),
                    f.Commerce.ProductDescription(),
                    f.Commerce.ProductDescription(),
                    f.Commerce.Ean8() + ".png",
                    f.Random.Number(100, 1000)));
    
    [Fact]
    public void Get_ShouldCallGetString()
    {
        // Arrange
        var product = GetFakeProduct.Generate(1).First();
        var json = JsonSerializer.SerializeToUtf8Bytes(product);
        
        var mockDistributedCache = new Mock<IDistributedCache>();
        mockDistributedCache.Setup(x => 
            x.Get(It.IsAny<string>()))
                .Returns(json);

        // Act
        var cacheProvider = new RedisCacheProvider(mockDistributedCache.Object);
        cacheProvider.Get<Product>(It.IsAny<string>());

        // Assert
        mockDistributedCache.Verify(x => x.Get(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public void Get_ShouldReturnObject()
    {
        // Arrange
        var product = GetFakeProduct.Generate(1).First();
        var json = JsonSerializer.SerializeToUtf8Bytes(product);
        
        var mockDistributedCache = new Mock<IDistributedCache>();
        mockDistributedCache.Setup(x => 
                x.Get(It.IsAny<string>()))
            .Returns(json);

        // Act
        var cacheProvider = new RedisCacheProvider(mockDistributedCache.Object);
        var result = cacheProvider.Get<Product>(It.IsAny<string>());

        // Assert
        result?.Id.Should().Be(product.Id);
    }    
    
    [Fact]
    public void Set_ShouldCallSetString()
    {
        // Arrange
        var product = GetFakeProduct.Generate(1).First();
        var json = JsonSerializer.SerializeToUtf8Bytes(product);

        var mockDistributedCache = new Mock<IDistributedCache>();
        mockDistributedCache.Setup(x =>
            x.Set(It.IsAny<string>(), json, It.IsAny<DistributedCacheEntryOptions>()));

        // Act
        var cacheProvider = new RedisCacheProvider(mockDistributedCache.Object);
        cacheProvider.Set<Product>(product.Id, product);

        // Assert
        mockDistributedCache.Verify(x => x.Set(It.IsAny<string>(), json, It.IsAny<DistributedCacheEntryOptions>()), Times.Once);
    }
}
