using Catalog.EmailWorker;
using Common.Logging;
using RabbitMQ.Client;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .UseSerilog(SeriLogger.Configure)
    .ConfigureServices(services =>
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        services.AddSingleton<IConnectionFactory>(connectionFactory);

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();