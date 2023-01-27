using Bogus;
using Catalog.Core.Data.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.UnitTests.Data;
public class DatabaseSettingsModelTests
{
    [Fact]
    public void DatabaseSettingsModel_OnCreation_ObjectCreated()
    {

        //Arrange
        var settingsFaker = new Faker<DatabaseSettingsModel>()
            .RuleFor(s => s.ConnectionString, f => f.Random.Uuid().ToString())
            .RuleFor(s => s.DatabaseName, f => f.Random.Uuid().ToString())
            .RuleFor(s => s.CollectionName, f => f.Random.Uuid().ToString());

        var settings = settingsFaker.Generate();

        //Act
        var sut = new DatabaseSettingsModel
        {
            ConnectionString = settings.ConnectionString,
            DatabaseName = settings.DatabaseName,
            CollectionName = settings.CollectionName,
        };

        //Assert
        sut.ConnectionString.Should().Be(settings.ConnectionString);
        sut.DatabaseName.Should().Be(settings.DatabaseName);
        sut.CollectionName.Should().Be(settings.CollectionName);
    }

}
