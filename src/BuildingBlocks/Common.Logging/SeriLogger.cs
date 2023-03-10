using Microsoft.Extensions.Hosting;
using Serilog;

namespace Common.Logging;

public static class SeriLogger
{
    public static Action<HostBuilderContext, LoggerConfiguration>
        Configure => (ctx, loggerConfiguration) =>
    {
        var elasticConfig = ElasticConfigurationBuilder.Build(ctx.Configuration, ctx);

        loggerConfiguration
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Environment", elasticConfig.EnvironmentName)
            .Enrich.WithProperty("Application", elasticConfig.ApplicationName)
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Elasticsearch(elasticConfig.ElasticsearchSinkOptions)
            .MinimumLevel.Information()
            .ReadFrom.Configuration(ctx.Configuration);
    };
}