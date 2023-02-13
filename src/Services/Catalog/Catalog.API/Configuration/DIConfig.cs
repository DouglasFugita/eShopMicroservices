using Catalog.Core.Data;
using Catalog.Core.Data.Models;
using Catalog.Core.Repositories;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Catalog.API.Configuration;

public static class DIConfig
{
    public static void RegisterServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<DatabaseSettingsModel>(configuration.GetSection("DatabaseSettings"));
        services.AddSingleton<ICatalogContext, CatalogContext>();
        services.AddScoped<ICatalogRepository, CatalogRepository>();

        var tracingUri = configuration.GetValue<string>("TracingConfiguration:Uri");

        if (tracingUri is not null)
        {
            services.AddOpenTelemetry()
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
                options.Endpoint = new Uri(tracingUri))
            )
            .StartWithHost();

        }
    }
}
