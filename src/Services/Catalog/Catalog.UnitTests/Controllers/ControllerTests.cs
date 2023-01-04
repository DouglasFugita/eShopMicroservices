using Catalog.API.Controllers;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Catalog.UnitTests.Fixtures;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Catalog.UnitTests.Controllers;

public class ControllerTests
{

    [Fact]
    public async Task GetProducts_OnSuccess_ReturnStatusCode200()
    {
        //Arrange
        var mockCatalogRepository = new Mock<ICatalogRepository>();
        mockCatalogRepository.Setup(service => service.GetProducts())
            .ReturnsAsync(ProductsFixtures.GetProductsFixture());

        var controller = new CatalogController(mockCatalogRepository.Object);

        //Act
        var actionResult = await controller.GetProducts();

        //Assert
        var result = actionResult.Result as OkObjectResult;
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeOfType<List<Product>>();
    }

    [Fact]
    public async Task GetProducts_OnNoProductsFound_ReturnsNotFound()
    {
        //Arrange
        var mockCatalogRepository = new Mock<ICatalogRepository>();
        mockCatalogRepository.Setup(service => service.GetProducts()).ReturnsAsync(new List<Product>());

        var controller = new CatalogController(mockCatalogRepository.Object);

        //Act
        var actionResult = await controller.GetProducts();

        //Assert
        var result = actionResult.Result as NotFoundResult;
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetProductById_OnSuccess_ReturnStatusCode200()
    {
        //Arrange
        var mockCatalogRepository = new Mock<ICatalogRepository>();
        mockCatalogRepository.Setup(service => service.GetProductById("string"))
            .ReturnsAsync(ProductsFixtures.GetFakeProduct.Generate(2).First());

        var controller = new CatalogController(mockCatalogRepository.Object);

        //Act
        var actionResult = await controller.GetProductById("string");

        //Assert
        var result = actionResult.Result as OkObjectResult;
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeOfType<Product>();
    }

    [Fact]
    public async Task GetProductById_OnNoProductsFound_ReturnsNotFound()
    {
        //Arrange
        var mockCatalogRepository = new Mock<ICatalogRepository>();
        mockCatalogRepository.Setup(service => service.GetProducts()).ReturnsAsync(new List<Product>());

        var controller = new CatalogController(mockCatalogRepository.Object);

        //Act
        var actionResult = await controller.GetProductById("string");

        //Assert
        var result = actionResult.Result as NotFoundResult;
        result.Should().BeOfType<NotFoundResult>();
    }
}