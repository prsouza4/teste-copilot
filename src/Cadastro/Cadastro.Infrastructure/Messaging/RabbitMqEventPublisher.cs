using System.Text;
using System.Text.Json;
using Cadastro.Application.Interfaces;
using Cadastro.Domain.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Cadastro.Infrastructure.Messaging;

/// <summary>
/// RabbitMQ implementation of IEventPublisher.
/// </summary>
public class RabbitMqEventPublisher : IEventPublisher, IDisposable
{
    private readonly ILogger<RabbitMqEventPublisher> _logger;
    private readonly IConnection? _connection;
    private readonly IModel? _channel;
    private readonly bool _isConnected;
    private bool _disposed;

    public RabbitMqEventPublisher(IConfiguration configuration, ILogger<RabbitMqEventPublisher> logger)
    {
        _logger = logger;

        var host = configuration["RabbitMQ:Host"] ?? "localhost";
        var port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672");
        var username = configuration["RabbitMQ:Username"] ?? "guest";
        var password = configuration["RabbitMQ:Password"] ?? "guest";

        try
        {
            var factory = new ConnectionFactory
            {
                HostName = host,
                Port = port,
                UserName = username,
                Password = password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare exchange for domain events
            _channel.ExchangeDeclare(
                exchange: "cadastro.events",
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false);

            _isConnected = true;
            _logger.LogInformation("Connected to RabbitMQ at {Host}:{Port}", host, port);
        }
        catch (Exception ex)
        {
            _isConnected = false;
            _logger.LogWarning(ex, "Failed to connect to RabbitMQ at {Host}:{Port}. Events will not be published.", host, port);
        }
    }

    public Task PublishAsync(DomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        if (!_isConnected || _channel is null)
        {
            _logger.LogWarning("RabbitMQ is not connected. Event {EventType} will not be published.", domainEvent.GetType().Name);
            return Task.CompletedTask;
        }

        try
        {
            var eventName = domainEvent.GetType().Name;
            var routingKey = $"cadastro.{eventName.ToLowerInvariant()}";

            var message = JsonSerializer.Serialize(domainEvent, domainEvent.GetType(), new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var body = Encoding.UTF8.GetBytes(message);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";
            properties.MessageId = domainEvent.Id.ToString();
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            properties.Type = eventName;

            _channel.BasicPublish(
                exchange: "cadastro.events",
                routingKey: routingKey,
                basicProperties: properties,
                body: body);

            _logger.LogInformation(
                "Published event {EventType} with ID {EventId} to routing key {RoutingKey}",
                eventName,
                domainEvent.Id,
                routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish event {EventType}", domainEvent.GetType().Name);
        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (_disposed) return;

        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();

        _disposed = true;
        GC.SuppressFinalize(this);
    }
}
