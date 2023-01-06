using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.UnitTests.Fixtures;
using FluentAssertions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.UnitTests.Data;
public class CatalogContextSeedTests
{

    [Fact]
    public async Task SeedData_OnAnyData_DoNotCallInsertMethod()
    {
        // Arrange
        var countDocumentsReturn = 1;
        var mockproductCollection = MockProductCollection(countDocumentsReturn);

        // Act
        await CatalogContextSeed.SeedData(mockproductCollection.Object);

        // Assert
        mockproductCollection.Verify(x => x.InsertManyAsync(It.IsAny<IEnumerable<Product>>(), It.IsAny<InsertManyOptions>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task SeedData_OnNoData_CallInsertMethod()
    {
        // Arrange
        var countDocumentsReturn = 0;
        var mockproductCollection = MockProductCollection(countDocumentsReturn);
        // Act
        await CatalogContextSeed.SeedData(mockproductCollection.Object);

        // Assert
        mockproductCollection.Verify(x => x.InsertManyAsync(It.IsAny<IEnumerable<Product>>(), It.IsAny<InsertManyOptions>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    public static Mock<IMongoCollection<Product>> MockProductCollection (int countDocumentReturns)
    {
        var MockProductCollection = new Mock<IMongoCollection<Product>>();
        MockProductCollection.Setup(
            x => x.InsertManyAsync(It.IsAny<IEnumerable<Product>>(), It.IsAny<InsertManyOptions>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockProductCollection.Setup(
            x => x.CountDocuments(It.IsAny<FilterDefinition<Product>>(), It.IsAny<CountOptions>(), It.IsAny<CancellationToken>()))
            .Returns(countDocumentReturns);

        return MockProductCollection;
    }

}
