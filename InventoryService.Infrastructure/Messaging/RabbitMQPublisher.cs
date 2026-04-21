using InventoryService.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace InventoryService.Infrastructure.Messaging
{
    public class RabbitMqPublisher : IMessagePublisher
    {
        private readonly Task<IConnection> _connectionTask;
        private IChannel? _channel;
        private const string ExchangeName = "book-exchange";
                
        public RabbitMqPublisher(Task<IConnection> connectionTask)
        {
            _connectionTask = connectionTask;
        }

        private async Task EnsureChannelIsCreatedAsync()
        {
            if (_channel == null)
            {                
                var connection = await _connectionTask;
                _channel = await connection.CreateChannelAsync();                                
                await _channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic, true);
            }
        }

        public async Task PublishAsync(object message)
        {
            await EnsureChannelIsCreatedAsync();

            string messageType = message.GetType().Name;
            string routingKey = messageType;

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            await _channel.BasicPublishAsync(
                exchange: ExchangeName,
                routingKey: routingKey,
                body: body);

            Console.WriteLine($"Published message with key: {routingKey}");
        }
    }
}
