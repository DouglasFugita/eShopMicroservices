using Catalog.Minimal.API.Configuration;
using Catalog.Minimal.API.Products;
using Common.Logging;
using MongoDB.Driver;
using Polly;
using RabbitMQ.Client;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices(builder.Configuration);

var connectionFactory = new ConnectionFactory
{
    Uri = new Uri(builder.Configuration.GetValue<string>("QueueSettings:ConnectionString"))
};

builder.Services.AddSingleton<IConnectionFactory>(connectionFactory);
builder.Services.AddSingleton(sp => Policy
    .Handle<RabbitMQ.Client.Exceptions.BrokerUnreachableException>()
    .WaitAndRetry(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
    .Execute(() => sp.GetRequiredService<IConnectionFactory>().CreateConnection())
);
builder.Services.AddTransient(sp => sp.GetRequiredService<IConnection>().CreateModel());


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog(SeriLogger.Configure);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCatalogEndpoints();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.Run();