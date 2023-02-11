using Catalog.Core.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

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
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        var exchange = "catalog_exchange";
        var queue = "catalog_queue";
        var routingKey = "catalog_routingkey";

        channel.ExchangeDeclare(exchange, ExchangeType.Direct);
        channel.QueueDeclare(queue, true, false, false, null);
        channel.QueueBind(queue, exchange, routingKey);
        channel.BasicQos(0, 2, false);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = System.Text.Encoding.UTF8.GetString(body);
            Product? product = null;

            try
            {
                product = JsonSerializer.Deserialize<Product>(message);
            } catch(Exception)
            {
                channel.BasicReject(ea.DeliveryTag, false);
                _logger.LogError("Email Worker - Warning:{Event} | Worker: {Worker} | Message:{message}", "Message reject due deserialization error", ea.ConsumerTag, message);
                return;
            }
            
            _logger.LogInformation("Email Worker - Event:{Event} | Worker: {Worker} | Message:{message}", "Sending Email", ea.ConsumerTag, message);
            await Task.Delay(10000, stoppingToken);
            channel.BasicAck(ea.DeliveryTag, false);
            _logger.LogInformation("Email Worker - Event:{Event} | Worker: {Worker} | Message Acked:{message}","Email Sent", ea.ConsumerTag, message);
        };

        channel.BasicConsume(queue, autoAck: false, consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(10000, stoppingToken);
        }
    }
}
