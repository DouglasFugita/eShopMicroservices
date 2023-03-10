using Catalog.Core.Entities;
using Catalog.Minimal.API.Products;
using Catalog.UnitTests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Catalog.UnitTests.MinimalAPI;

public class ProductEndpointsTests
{
    [Fact]
    public async Task GetAllProducts_ReturnsOkResult()
    {
        //Arrange
        var mockProductService = new Mock<IProductService>();
        mockProductService.Setup(service => service.GetProducts())
            .ReturnsAsync(ProductsFixtures.GetProductsFixture());

        //Act
        var result = await ProductEndpoints.GetAllProducts(mockProductService.Object);

        //Assert
        result.Should().BeOfType<Ok<IEnumerable<Product>>>();
        var okResult = (Ok<IEnumerable<Product>>)result;
        okResult.Value.Should().BeOfType<List<Product>>();
    }
    [Fact]
    public async Task GetAllProducts_ReturnsNotFoundResult()
    {
        //Arrange
        var mockProductService = new Mock<IProductService>();
        mockProductService.Setup(service => service.GetProducts())
            .ReturnsAsync(new List<Product>());

        //Act
        var result = await ProductEndpoints.GetAllProducts(mockProductService.Object);

        //Assert
        result.Should().BeOfType<NotFound>();
    }    
}