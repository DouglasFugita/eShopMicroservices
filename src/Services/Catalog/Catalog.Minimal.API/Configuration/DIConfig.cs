using Catalog.Core.Data;
using Catalog.Core.Data.Models;
using Catalog.Core.Repositories;
using Catalog.Minimal.API.Products;
using Common.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RabbitMQ.Client;
using StackExchange.Redis;

namespace Catalog.Minimal.API.Configuration;

public static class DIConfig
{
    public static void RegisterServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<DatabaseSettingsModel>(configuration.GetSection("DatabaseSettings"));

        var redisUri = configuration.GetValue<string>("CacheSettings:ConnectionString");
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisUri;
            options.InstanceName = "CatalogRedisCache";
        });

        services.AddSingleton<IRedisCacheProvider, RedisCacheProvider>();

        var connectionFactory = new ConnectionFactory
        {
            Uri = new Uri(configuration.GetValue<string>("QueueSettings:ConnectionString"))
        };
        services.AddSingleton<IConnectionFactory>(connectionFactory);



        services.AddSingleton<ICatalogContext, CatalogContext>();
        services.AddScoped<ICatalogRepository, CatalogRepository>();
        services.AddScoped<IProductService, ProductService>();

        var tracingUri = configuration.GetValue<string>("TracingConfiguration:Uri");
        if (tracingUri is not null)
        {
            services.AddOpenTelemetry()
                .WithMetrics(metricBuilder => metricBuilder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddPrometheusExporter())
                .WithTracing(tracingBuilder => tracingBuilder
                    .AddSource("Catalog")
                    .SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                            .AddService(serviceName: "CatalogMinimalAPI", serviceVersion: "1.0")
                            .AddTelemetrySdk())
                    .AddAspNetCoreInstrumentation()
                    .AddMongoDBInstrumentation()
                    .ConfigureBuilder((sp, configure) =>
                    {
                        RedisCache redisCache = (RedisCache)sp.GetRequiredService<IDistributedCache>();
                        configure.AddRedisInstrumentation(redisCache.GetConnection());
                    })
                    .AddOtlpExporter(options =>
                options.Endpoint = new Uri(tracingUri))
            )
            .StartWithHost();

        }
    }
}
