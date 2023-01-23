using Catalog.Core.Entities;
using Catalog.UnitTests.Fixtures;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;

namespace Catalog.UnitTests.Repositories;
public class CatalogRepositoryTest : IClassFixture<CatalogContextFixture>
{
    private readonly CatalogContextFixture _fixture;

    public CatalogRepositoryTest(CatalogContextFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CreateProduct_OnSuccess_InsertOneAsyncMethodCalled()
    {
        //Arrange
        _fixture.MockProductCollection
            .Setup(
                x => x.InsertOneAsync(It.IsAny<Product>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act
        await _fixture.CatalogRepository.CreateProduct(_fixture.ExampleProduct);

        //Assert
        _fixture.MockProductCollection.Verify(x => x.InsertOneAsync(It.IsAny<Product>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task DeleteProduct_OnSuccess_DeleteOneAsyncMethodCalled()
    {
        //Arrange
        _fixture.MockProductCollection
            .Setup(
                x => x.DeleteOneAsync(It.IsAny<FilterDefinition<Product>>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeleteResult.Acknowledged(1));
        //Act
        await _fixture.CatalogRepository.DeleteProduct("id");

        //Assert
        _fixture.MockProductCollection.Verify(x => x.DeleteOneAsync(It.IsAny<FilterDefinition<Product>>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task UpdateProduct_OnSuccess_ReplaceOneAsyncMethodCalled()
    {
        //Arrange
        _fixture.MockProductCollection
            .Setup(
                x => x.ReplaceOneAsync(It.IsAny<FilterDefinition<Product>>(), It.IsAny<Product>(),
                It.IsAny<ReplaceOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReplaceOneResult.Acknowledged(1,1,new BsonString("string")));

        //Act
        await _fixture.CatalogRepository.UpdateProduct(_fixture.ExampleProduct);

        //Assert
        _fixture.MockProductCollection.Verify(x => x.ReplaceOneAsync(It.IsAny<FilterDefinition<Product>>(), It.IsAny<Product>(),
                It.IsAny<ReplaceOptions>(), It.IsAny<CancellationToken>()), Times.Once());
    }


    [Fact]
    public async Task GetProducts_OnSuccess_FindMethodCalled()
    {
        //Arrange
        _fixture.MockProductCollection
            .Setup(
                x => x.FindAsync(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions<Product, Product>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.MockCursor.Object);

        //Act
        await _fixture.CatalogRepository.GetProducts();

        //Assert
        _fixture.MockProductCollection.Verify(x => x.FindAsync(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions<Product, Product>>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task GetProductsByName_OnSuccess_FindMethodCalled()
    {
        //Arrange
        _fixture.MockProductCollection
            .Setup(
                x => x.FindAsync(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions<Product, Product>>(), It.IsAny<CancellationToken>())
                )
            .ReturnsAsync(_fixture.MockCursor.Object);

        //Act
        await _fixture.CatalogRepository.GetProductByName(_fixture.FilterParam);

        //Assert
        _fixture.MockProductCollection.Verify(x => x.FindAsync(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions<Product, Product>>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task GetProductsByName_OnSuccess_WithValidFilter()
    {
        //Arrange
        var renderedExpectedFilter = CatalogContextFixture.FilterRender(Builders<Product>.Filter.Eq(p => p.Name, _fixture.FilterParam));
        var renderedCreatedFilter = new BsonDocument();

        _fixture.MockProductCollection
            .Setup(
                x => x.FindAsync(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions<Product, Product>>(), It.IsAny<CancellationToken>())
                )
            .Callback<FilterDefinition<Product>, FindOptions<Product, Product>, CancellationToken>(
                (x, y, z) => {
                    renderedCreatedFilter = CatalogContextFixture.FilterRender(x);
                })
            .ReturnsAsync(_fixture.MockCursor.Object);

        //Act
        await _fixture.CatalogRepository.GetProductByName(_fixture.FilterParam);

        //Assert
        renderedExpectedFilter.Should().Equal(renderedCreatedFilter);
    }

    [Fact]
    public async Task GetProductsById_OnSuccess_FindMethodCalled()
    {
        //Arrange
        _fixture.MockProductCollection
            .Setup(
                x => x.FindAsync(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions<Product, Product>>(), It.IsAny<CancellationToken>())
                )
            .ReturnsAsync(_fixture.MockCursor.Object);

        //Act
        await _fixture.CatalogRepository.GetProductById(_fixture.FilterParam);

        //Assert
        _fixture.MockProductCollection.Verify(x => x.FindAsync(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions<Product, Product>>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task GetProductsById_OnSuccess_WithValidFilter()
    {
        //Arrange
        var renderedExpectedFilter = CatalogContextFixture.FilterRender(Builders<Product>.Filter.Eq(p => p.Id, _fixture.FilterIdParam));
        var renderedCreatedFilter = new BsonDocument();

        _fixture.MockProductCollection
            .Setup(
                x => x.FindAsync(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions<Product, Product>>(), It.IsAny<CancellationToken>())
                )
            .Callback<FilterDefinition<Product>, FindOptions<Product, Product>, CancellationToken>(
                (x, y, z) => {
                    renderedCreatedFilter = CatalogContextFixture.FilterRender(x);
                })
            .ReturnsAsync(_fixture.MockCursor.Object);

        //Act
        await _fixture.CatalogRepository.GetProductById(_fixture.FilterIdParam);

        //Assert
        renderedExpectedFilter.Should().Equal(renderedCreatedFilter);
    }

    [Fact]
    public async Task GetProductsByCategory_OnSuccess_FindMethodCalled()
    {
        //Arrange
        _fixture.MockProductCollection
            .Setup(
                x => x.FindAsync(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions<Product, Product>>(), It.IsAny<CancellationToken>())
                )
            .ReturnsAsync(_fixture.MockCursor.Object);

        //Act
        await _fixture.CatalogRepository.GetProductByCategory(_fixture.FilterParam);

        //Assert
        _fixture.MockProductCollection.Verify(x => x.FindAsync(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions<Product, Product>>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task GetProductsByCategory_OnSuccess_WithValidFilter()
    {
        //Arrange
        var renderedExpectedFilter = CatalogContextFixture.FilterRender(Builders<Product>.Filter.Eq(p => p.Category, _fixture.FilterParam));
        var renderedCreatedFilter = new BsonDocument();

        _fixture.MockProductCollection
            .Setup(
                x => x.FindAsync(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions<Product, Product>>(), It.IsAny<CancellationToken>())
                )
            .Callback<FilterDefinition<Product>, FindOptions<Product, Product>, CancellationToken>(
                (x, y, z) => {
                    renderedCreatedFilter = CatalogContextFixture.FilterRender(x);
                })
            .ReturnsAsync(_fixture.MockCursor.Object);

        //Act
        await _fixture.CatalogRepository.GetProductByCategory(_fixture.FilterParam);

        //Assert
        renderedExpectedFilter.Should().Equal(renderedCreatedFilter);
    }
}
