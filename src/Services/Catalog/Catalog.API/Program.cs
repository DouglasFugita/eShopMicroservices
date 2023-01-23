using Catalog.API.Configuration;
using Common.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenTelemetry()
    .WithMetrics(metricBuilder => metricBuilder
        .AddAspNetCoreInstrumentation()
        .AddPrometheusExporter())
    .WithTracing(tracingBuilder => tracingBuilder
        .AddSource("Catalog")
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName: "Catalog", serviceVersion: "1.0")
                .AddTelemetrySdk())
        .AddAspNetCoreInstrumentation()
        .AddMongoDBInstrumentation()
        .AddOtlpExporter(options =>
            options.Endpoint = new Uri(builder.Configuration.GetValue<string>("JaegerConfiguration:Uri")))
        )
    .StartWithHost();

builder.Services.AddControllers();
builder.Services.RegisterServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog(SeriLogger.Configure);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseAuthorization();

app.MapControllers();

app.Run();


