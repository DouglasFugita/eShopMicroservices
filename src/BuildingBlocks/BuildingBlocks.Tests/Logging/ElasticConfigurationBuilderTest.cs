using Common.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Tests.Logging;

public class ElasticConfigurationBuilderTest
{
    [Fact]
    public void Create_OnSuccess_ShouldGenerateElasticConfiguration()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Configuration.AddJsonFile("./Logging/appsettingsfake.json");

        //Act / Assert
#pragma warning disable ASP0013 // Suggest switching from using Configure methods to WebApplicationBuilder.Configuration
        builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
        {
            var sut = ElasticConfigurationBuilder.Build(config.Build(), hostingContext);
            sut.ProjectName.Should().Be("eShopMicroservices");
            sut.EnvironmentName.Should().Be("production");
        });
#pragma warning restore ASP0013 // Suggest switching from using Configure methods to WebApplicationBuilder.Configuration
    }
}