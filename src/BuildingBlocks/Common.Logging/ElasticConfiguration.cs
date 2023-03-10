using Serilog.Sinks.Elasticsearch;

namespace Common.Logging;

public class ElasticConfiguration
{
    public ElasticConfiguration(string projectName, string environmentName, string applicationName,
        ElasticsearchSinkOptions elasticsearchSinkOptions)
    {
        ProjectName = projectName;
        EnvironmentName = environmentName;
        ElasticsearchSinkOptions = elasticsearchSinkOptions;
        ApplicationName = applicationName;
    }

    public string ProjectName { get; }
    public string EnvironmentName { get; }
    public string ApplicationName { get; }
    public ElasticsearchSinkOptions ElasticsearchSinkOptions { get; }
}