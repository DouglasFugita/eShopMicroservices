using Catalog.Core.Entities;
using Catalog.Minimal.API.Products;
using Catalog.UnitTests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
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
    [Fact]
    public async Task GetProductById_ReturnsOkResult()
    {
        //Arrange
        var mockProductService = new Mock<IProductService>();
        mockProductService.Setup(service => service.GetProductById(It.IsAny<string>()))
            .ReturnsAsync(ProductsFixtures.GetFakeProduct);

        //Act
        var result = await ProductEndpoints.GetProductById(mockProductService.Object, It.IsAny<string>());

        //Assert
        result.Should().BeOfType<Ok<Product>>();
        var okResult = (Ok<Product>)result;
        okResult.Value.Should().BeOfType<Product>();
    }
    [Fact]
    public async Task GetProductById_ReturnsNoFoundResult()
    {
        //Arrange
        var mockProductService = new Mock<IProductService>();
        mockProductService.Setup(service => service.GetProductById(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        //Act
        var result = await ProductEndpoints.GetProductById(mockProductService.Object, It.IsAny<string>());

        //Assert
        result.Should().BeOfType<NotFound>();
    } 
    [Fact]
    public async Task GetProductsByCategory_ReturnsOkResult()
    {
        //Arrange
        var mockProductService = new Mock<IProductService>();
        mockProductService.Setup(service => service.GetProductsByCategory(It.IsAny<string>()))
            .ReturnsAsync(ProductsFixtures.GetProductsFixture());

        //Act
        var result = await ProductEndpoints.GetProductsByCategory(mockProductService.Object, It.IsAny<string>());

        //Assert
        result.Should().BeOfType<Ok<IEnumerable<Product>>>();
        var okResult = (Ok<IEnumerable<Product>>)result;
        okResult.Value.Should().BeOfType<List<Product>>();
    }
    [Fact]
    public async Task GetProductsByCategory_ReturnsNotFoundResult()
    {
        //Arrange
        var mockProductService = new Mock<IProductService>();
        mockProductService.Setup(service => service.GetProductsByCategory(It.IsAny<string>()))
            .ReturnsAsync(new List<Product>());

        //Act
        var result = await ProductEndpoints.GetProductsByCategory(mockProductService.Object, It.IsAny<string>());

        //Assert
        result.Should().BeOfType<NotFound>();
    }
    [Fact]
    public async Task GetProductsByName_ReturnsOkResult()
    {
        //Arrange
        var mockProductService = new Mock<IProductService>();
        mockProductService.Setup(service => service.GetProductsByName(It.IsAny<string>()))
            .ReturnsAsync(ProductsFixtures.GetProductsFixture());

        //Act
        var result = await ProductEndpoints.GetProductsByName(mockProductService.Object, It.IsAny<string>());

        //Assert
        result.Should().BeOfType<Ok<IEnumerable<Product>>>();
        var okResult = (Ok<IEnumerable<Product>>)result;
        okResult.Value.Should().BeOfType<List<Product>>();
    }
    [Fact]
    public async Task GetProductsByName_ReturnsNotFoundResult()
    {
        //Arrange
        var mockProductService = new Mock<IProductService>();
        mockProductService.Setup(service => service.GetProductsByName(It.IsAny<string>()))
            .ReturnsAsync(new List<Product>());

        //Act
        var result = await ProductEndpoints.GetProductsByName(mockProductService.Object, It.IsAny<string>());

        //Assert
        result.Should().BeOfType<NotFound>();
    }
    
    [Fact]
    public void CreateProduct_ReturnsCreatedAtRoute()
    {
        //Arrange
        var product = ProductsFixtures.GetFakeProduct.Generate(1).First();

        var mockCatalogRepository = new Mock<IProductService>();
        mockCatalogRepository.Setup(service => service.CreateProduct(It.IsAny<Product>()));

        //Act
        var result = ProductEndpoints.CreateProduct(mockCatalogRepository.Object, product);

        //Assert
        result.Should().BeOfType<CreatedAtRoute<Product>>();
        var createResult = (CreatedAtRoute<Product>)result;
        createResult.Value.Should().BeOfType<Product>();
    }
    [Fact]
    public void UpdateProduct_ReturnsAcceptedAtRoute()
    {
        //Arrange
        var product = ProductsFixtures.GetFakeProduct.Generate(1).First();

        var mockCatalogRepository = new Mock<IProductService>();
        mockCatalogRepository.Setup(service => service.UpdateProduct(It.IsAny<Product>()));

        //Act
        var result = ProductEndpoints.UpdateProduct(mockCatalogRepository.Object, product);

        //Assert
        result.Should().BeOfType<AcceptedAtRoute<Product>>();
        var createResult = (AcceptedAtRoute<Product>)result;
        createResult.Value.Should().BeOfType<Product>();
    }       

    [Fact]
    public void DeleteProduct_ReturnsOkResult()
    {
        //Arrange
        var mockCatalogRepository = new Mock<IProductService>();
        mockCatalogRepository.Setup(service => service.DeleteProduct(It.IsAny<string>()));

        //Act
        var result = ProductEndpoints.DeleteProduct(mockCatalogRepository.Object, It.IsAny<string>());

        //Assert
        result.Should().BeOfType<Ok>();
    }
}