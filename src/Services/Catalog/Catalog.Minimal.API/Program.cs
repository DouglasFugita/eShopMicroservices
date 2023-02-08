using Catalog.Minimal.API.Configuration;
using Catalog.Minimal.API.Products;
using Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices(builder.Configuration);



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
