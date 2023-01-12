using Common.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
