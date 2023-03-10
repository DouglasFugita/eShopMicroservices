using Common.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace BuildingBlocks.Tests.Logging;

public class SeriLoggerTest
{
    [Fact]
    public void Create_OnSuccess_ShouldGenerateElasticConfiguration()
    {
        //Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Configuration.AddJsonFile("./Logging/appsettingsfake.json");

        //Act
        builder.Host.UseSerilog(SeriLogger.Configure);

        // Assert
        var app = builder.Build();
        app.Logger.GetType().FullName.Should().Be("Serilog.Extensions.Logging.SerilogLogger");
    }
}