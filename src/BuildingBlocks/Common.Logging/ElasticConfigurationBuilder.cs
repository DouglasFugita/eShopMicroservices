using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace Common.Logging;
public static class ElasticConfigurationBuilder
{
    public static ElasticConfiguration Build(IConfiguration config, HostBuilderContext ctx)
    {
        var route = "ElasticConfiguration";
        var defaultProjectName = "eShopMicroservices";
        var section = config.GetSection(route);

        if (!section.Exists())
            throw new ArgumentException($"ElasticConfigurationBuilder - Section {route} not found");


        var stringUri = section.GetValue<string>("Uri");

        if (stringUri is null)
            throw new ArgumentException("ElasticConfigurationBuilder - Uri not found");

        var hostUri = new Uri(stringUri);
        var projectName = section.GetValue<string?>($"{route}:ProjectName") ?? defaultProjectName;
        var environmentName = ctx.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-") ?? "Unknown";
        var applicationName = ctx.HostingEnvironment.ApplicationName ?? "Unknown";
        var autoRegisterTemplate = section.GetValue<bool?>($"{route}:AutoRegisterTemplate") ?? true;
        var numberOfShards = section.GetValue<int?>($"{route}:NumberOfShards") ?? 1;
        var numberOfReplicas = section.GetValue<int?>($"{route}:NumberOfReplicas") ?? 0;

        var options = new ElasticsearchSinkOptions(hostUri)
        {
            IndexFormat = $"applogs-{projectName}-{environmentName}-logs-{DateTime.UtcNow:yyyy-MM}",
            AutoRegisterTemplate = autoRegisterTemplate,
            NumberOfShards = numberOfShards,
            NumberOfReplicas = numberOfReplicas
        };

        return new ElasticConfiguration(projectName, environmentName, applicationName, options);
    }
}
