using Catalog.API.Controllers;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Catalog.UnitTests.Fixtures;
using FluentAssertions;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using System.Text;

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
        if (result is not null)
        {
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<List<Product>>();
        } else
        {
            Assert.Fail("result is null");
        }
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
        mockCatalogRepository.Setup(service => service.GetProductById(It.IsAny<String>()))
            .ReturnsAsync(ProductsFixtures.GetFakeProduct.Generate(1).First());

        var controller = new CatalogController(mockCatalogRepository.Object);

        //Act
        var actionResult = await controller.GetProductById(It.IsAny<String>());

        //Assert
        var result = actionResult.Result as OkObjectResult;
        if (result is not null)
        {
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<Product>();
        }
        else
        {
            Assert.Fail("result is null");
        }
    }

    [Fact]
    public async Task GetProductById_OnNoProductFound_ReturnsNotFound()
    {
        //Arrange
        var mockCatalogRepository = new Mock<ICatalogRepository>();
        mockCatalogRepository.Setup(service => service.GetProductById(It.IsAny<String>()))
            .ReturnsAsync(() => null);

        var controller = new CatalogController(mockCatalogRepository.Object);

        //Act
        var actionResult = await controller.GetProductById(It.IsAny<String>());

        //Assert
        var result = actionResult.Result as NotFoundResult;
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetProductByCategory_OnSuccess_ReturnStatusCode200()
    {
        //Arrange
        var mockCatalogRepository = new Mock<ICatalogRepository>();
        mockCatalogRepository.Setup(service => service.GetProductByCategory(It.IsAny<String>()))
            .ReturnsAsync(ProductsFixtures.GetFakeProduct.Generate(2));

        var controller = new CatalogController(mockCatalogRepository.Object);

        //Act
        var actionResult = await controller.GetProductByCategory(It.IsAny<String>());

        //Assert
        var result = actionResult.Result as OkObjectResult;
        if (result is not null)
        {
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<List<Product>>();
        }
        else
        {
            Assert.Fail("result is null");
        }
    }

    [Fact]
    public async Task GetProductByCategory_OnNoProductsFound_ReturnsNotFound()
    {
        //Arrange
        var mockCatalogRepository = new Mock<ICatalogRepository>();
        mockCatalogRepository.Setup(service => service.GetProductByCategory(It.IsAny<String>()))
            .ReturnsAsync(new List<Product>());

        var controller = new CatalogController(mockCatalogRepository.Object);

        //Act
        var actionResult = await controller.GetProductByCategory(It.IsAny<String>());

        //Assert
        var result = actionResult.Result as NotFoundResult;
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetProductByName_OnSuccess_ReturnStatusCode200()
    {
        //Arrange
        var mockCatalogRepository = new Mock<ICatalogRepository>();
        mockCatalogRepository.Setup(service => service.GetProductByName(It.IsAny<String>()))
            .ReturnsAsync(ProductsFixtures.GetFakeProduct.Generate(2).ToList());

        var controller = new CatalogController(mockCatalogRepository.Object);

        //Act
        var actionResult = await controller.GetProductByName(It.IsAny<String>());

        //Assert
        var result = actionResult.Result as OkObjectResult;
        if (result is not null)
        {
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<List<Product>>();
        }
        else
        {
            Assert.Fail("result is null");
        }
    }

    [Fact]
    public async Task GetProductByName_OnNoProductsFound_ReturnsNotFound()
    {
        //Arrange
        var mockCatalogRepository = new Mock<ICatalogRepository>();
        mockCatalogRepository.Setup(service => service.GetProductByName(It.IsAny<String>()))
            .ReturnsAsync(new List<Product>());

        var controller = new CatalogController(mockCatalogRepository.Object);

        //Act
        var actionResult = await controller.GetProductByName(It.IsAny<String>());

        //Assert
        var result = actionResult.Result as NotFoundResult;
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateProduct_OnSuccess_ReturnsStatusCode201()
    {
        //Arrange
        var produto = ProductsFixtures.GetFakeProduct.Generate(1).First();

        var mockCatalogRepository = new Mock<ICatalogRepository>();
        mockCatalogRepository.Setup(service => service.CreateProduct(It.IsAny<Product>()))
            .Returns(Task.CompletedTask);

        var controller = new CatalogController(mockCatalogRepository.Object);

        //Act
        var actionResult = await controller.CreateProduct(produto);

        //Assert
        var result = actionResult.Result as CreatedAtRouteResult;
        if (result is not null)
        {
            result.StatusCode.Should().Be(201);
            mockCatalogRepository.Verify(x => x.CreateProduct(It.IsAny<Product>()), Times.Once());
        }
        else
        {
            Assert.Fail("result is null");
        }
    }

    [Fact]
    public async Task UpdateProduct_OnSuccess_ReturnsStatusCode200()
    {
        //Arrange
        var mockCatalogRepository = new Mock<ICatalogRepository>();
        mockCatalogRepository.Setup(service => service.UpdateProduct(It.IsAny<Product>()))
            .ReturnsAsync(true);

        var controller = new CatalogController(mockCatalogRepository.Object);

        //Act
        var actionResult = await controller.UpdateProduct(It.IsAny<Product>());

        //Assert
        var result = actionResult as OkObjectResult;
        if (result is not null)
        {
            result.StatusCode.Should().Be(200);
            mockCatalogRepository.Verify(x => x.UpdateProduct(It.IsAny<Product>()), Times.Once());
        }
        else
        {
            Assert.Fail("result is null");
        }
    }

    [Fact]
    public async Task DeleteProduct_OnSuccess_ReturnsStatusCode200()
    {
        //Arrange
        var mockCatalogRepository = new Mock<ICatalogRepository>();
        mockCatalogRepository.Setup(service => service.DeleteProduct(It.IsAny<String>()))
            .ReturnsAsync(true);

        var controller = new CatalogController(mockCatalogRepository.Object);

        //Act
        var actionResult = await controller.DeleteProduct(It.IsAny<String>());

        //Assert
        var result = actionResult as OkObjectResult;
        if (result is not null)
        {
            result.StatusCode.Should().Be(200);
            mockCatalogRepository.Verify(x => x.DeleteProduct(It.IsAny<String>()), Times.Once());
        }
        else
        {
            Assert.Fail("result is null");
        }
    }
}