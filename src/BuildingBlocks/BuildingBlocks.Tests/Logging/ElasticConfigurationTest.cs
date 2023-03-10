using Common.Logging;
using FluentAssertions;
using Serilog.Sinks.Elasticsearch;

namespace BuildingBlocks.Tests.Logging;

public class ElasticConfigurationTest
{
    [Fact]
    public void Construct_OnSuccess_ShouldValidateProperties()
    {
        //Arrange
        var projectName = "Project";
        var environmentName = "Environment";
        var applicationName = "Application";
        var options = new ElasticsearchSinkOptions();

        // Act
        var sut = new ElasticConfiguration(projectName, environmentName, applicationName, options);

        //Assert
        sut.ProjectName.Should().Be(projectName);
        sut.EnvironmentName.Should().Be(environmentName);
        sut.ApplicationName.Should().Be(applicationName);
        sut.ElasticsearchSinkOptions.Should().Be(options);
    }
}