using Catalog.Core.Data;
using Catalog.Core.Data.Models;
using Catalog.Core.Repositories;
using Catalog.Minimal.API.Products;
using Common.Caching;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Catalog.Minimal.API.Configuration;

public static class DIConfig
{
    public static void RegisterServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<DatabaseSettingsModel>(configuration.GetSection("DatabaseSettings"));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetValue<string>("CacheSettings:ConnectionString");
            options.InstanceName = "CatalogRedisCache";
        });
        services.AddSingleton<IRedisCacheProvider, RedisCacheProvider>();

        services.AddSingleton<ICatalogContext, CatalogContext>();
        services.AddScoped<ICatalogRepository, CatalogRepository>();
        services.AddScoped<IProductService, ProductService>();

        var jaegerUri = configuration.GetValue<string>("JaegerConfiguration:Uri");
        if (jaegerUri is not null)
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
                options.Endpoint = new Uri(jaegerUri))
            )
            .StartWithHost();

        }
    }
}
