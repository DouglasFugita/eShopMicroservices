using Catalog.Minimal.API.Configuration;
using Catalog.Minimal.API.Products;
using Common.Logging;
using Polly;
using RabbitMQ.Client;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices(builder.Configuration);

var connectionFactory = new ConnectionFactory {
    HostName= "localhost",
    UserName = "guest",
    Password= "guest"
};

builder.Services.AddSingleton<IConnectionFactory>(connectionFactory);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog(SeriLogger.Configure);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapProductEndpoints();
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.Run();
