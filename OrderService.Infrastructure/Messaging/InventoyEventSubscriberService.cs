using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using EventBus.Messages;
using OrderService.Application.Interfaces;
using OrderService.Domain.Enums;

namespace OrderService.Infrastructure.Messaging
{
    public class InventoryEventSubscriberService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private IConnection? _connection;
        private IChannel? _channel;
        
        public InventoryEventSubscriberService(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancelToken)
        {
            var rabbitMqConnection = _configuration.GetConnectionString("RabbitMqConnection");
            var factory = new ConnectionFactory() { Uri = new Uri(rabbitMqConnection!) };
            
            _connection = await factory.CreateConnectionAsync(cancelToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancelToken);
                        
            await _channel.ExchangeDeclareAsync(exchange: "book-exchange", type: ExchangeType.Topic, durable: true, cancellationToken: cancelToken);
                        
            var queueName = "order-service-stock-queue";
            await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancelToken);                        
            await _channel.QueueBindAsync(queue: queueName, exchange: "book-exchange", routingKey: nameof(BookStockChangedIntegrationEvent), cancellationToken: cancelToken);
                        
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += ProcessMessageAsync;
            
            await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer, cancellationToken: cancelToken);
        }

        private async Task ProcessMessageAsync(object sender, BasicDeliverEventArgs @event)
        {
            try
            {                
                var body = @event.Body.ToArray();
                var messageText = Encoding.UTF8.GetString(body);
                var integrationEvent = JsonSerializer.Deserialize<BookStockChangedIntegrationEvent>(messageText);

                if (integrationEvent != null)
                {
                    Console.WriteLine($"- OrderService received stockQty update for BookId: {integrationEvent.BookId}, NewStock: {integrationEvent.NewStockQuantity}");
                                        
                    if (integrationEvent.NewStockQuantity == 0)
                    {
                        await PutPendingOrdersOnHoldAsync(integrationEvent.BookId);
                    }
                }
                
                if (_channel != null)
                {
                    await _channel.BasicAckAsync(deliveryTag: @event.DeliveryTag, multiple: false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"- Error processing message: {ex.Message}");                
            }
        }

        private async Task PutPendingOrdersOnHoldAsync(int outOfStockBookId)
        {            
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IOrderDbContext>();
                        
            var affectedOrders = await dbContext.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.Status == OrderStatus.Pending &&
                            o.OrderItems.Any(oi => oi.BookId == outOfStockBookId))
                .ToListAsync();

            if (affectedOrders.Any())
            {
                foreach (var order in affectedOrders)
                {
                    order.Status = OrderStatus.OnHold;
                    Console.WriteLine($"- OrderId: {order.Id} put OnHold because of BookId {outOfStockBookId} being out of stock.");
                }

                await dbContext.SaveChangesAsync(CancellationToken.None);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel != null) await _channel.CloseAsync(cancellationToken);
            if (_connection != null) await _connection.CloseAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }
    }
}
