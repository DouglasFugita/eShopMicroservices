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
    public async void GetProducts_OnSuccess_ReturnStatusCode200()
    {
        //Arrange
        var mockCatalogRepository = new Mock<ICatalogRepository>();
        mockCatalogRepository.Setup(service => service.GetProducts())
            .ReturnsAsync(ProductsFixtures.GetProductsFixture());

        var sut = new CatalogController(mockCatalogRepository.Object);

        //Act
        var result = await sut.GetProducts();

        //Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.StatusCode.Should().Be(200);
    }

    [Fact]
    public async void GetProducts_OnSuccess_InvokeRepositoryExactlyOnce()
    {
        //Arrange
        var mockCatalogRepository = new Mock<ICatalogRepository>();
        mockCatalogRepository.Setup(service => service.GetProducts())
            .ReturnsAsync(ProductsFixtures.GetProductsFixture());

        var sut = new CatalogController(mockCatalogRepository.Object);

        //Act
        await sut.GetProducts();

        //Assert
        mockCatalogRepository.Verify(service => service.GetProducts(), Times.Once()); 
    }

    [Fact]
    public async void GetProducts_OnSuccess_ReturnsListOfProducts()
    {
        //Arrange
        var mockCatalogRepository = new Mock<ICatalogRepository>();
        mockCatalogRepository.Setup(service => service.GetProducts())
            .ReturnsAsync(ProductsFixtures.GetProductsFixture());

        var sut = new CatalogController(mockCatalogRepository.Object);

        //Act
        var result = await sut.GetProducts();

        //Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<List<Product>>();
    }

    [Fact]
    public async void GetProducts_OnNoProductsFound_ReturnsNotFound()
    {
        //Arrange
        var mockCatalogRepository = new Mock<ICatalogRepository>();
        mockCatalogRepository.Setup(service => service.GetProducts()).ReturnsAsync(new List<Product>());

        var sut = new CatalogController(mockCatalogRepository.Object);

        //Act
        var result = await sut.GetProducts();

        //Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}