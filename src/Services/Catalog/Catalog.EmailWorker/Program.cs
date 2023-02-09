using Catalog.EmailWorker;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
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
