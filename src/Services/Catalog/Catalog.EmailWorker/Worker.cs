using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Catalog.EmailWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConnectionFactory _connectionFactory;

    public Worker(ILogger<Worker> logger, IConnectionFactory connectionFactory)
    {
        _logger = logger;
        _connectionFactory = connectionFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            var exchange = "catalog_exchange";
            var queue = "catalog_queue";
            var routingKey = "catalog_routingkey";

            channel.ExchangeDeclare(exchange, ExchangeType.Direct);
            channel.QueueDeclare(queue, true, false, false, null);
            channel.QueueBind(queue, exchange, routingKey);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Encoding.UTF8.GetString(body);
                _logger.LogInformation("Worker sending email... {message}",message);
            };

            channel.BasicConsume(queue, true, consumer);

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(10000, stoppingToken);
        }
    }
}
