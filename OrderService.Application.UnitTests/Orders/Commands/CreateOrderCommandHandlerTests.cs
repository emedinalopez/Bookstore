using Microsoft.EntityFrameworkCore;
using OrderService.Application.Orders.Commands;
using OrderService.Domain.Enums;
using OrderService.Infrastructure.Persistence;
using static OrderService.Application.Orders.Commands.CreateOrderCommand;

namespace OrderService.Application.UnitTests.Orders.Commands
{
    public class CreateOrderCommandHandlerTests
    {        
        private OrderDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new OrderDbContext(options);
        }

        [Fact]
        public async Task Handle_GivenValidCommand_ShouldCreateOrderAndOrderItems()
        {
            // Arrange
            await using var dbContext = GetInMemoryDbContext();
                        
            var command = new CreateOrderCommand
            {
                CustomerName = "John Doe",
                Items = new List<OrderItemCommand>
                {
                    new OrderItemCommand { BookId = 1, Quantity = 2, UnitPrice = 10.00m },
                    new OrderItemCommand { BookId = 5, Quantity = 1, UnitPrice = 25.50m }
                }
            };
                        
            var handler = new CreateOrderCommandHandler(dbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal("John Doe", result.CustomerName);
                        
            var orderInDb = await dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == result.Id);

            Assert.NotNull(orderInDb);

            Assert.Equal("John Doe", orderInDb.CustomerName);
            Assert.Equal(OrderStatus.Pending, orderInDb.Status);
            
            // Check that OrderDate is relatively recent
            Assert.True(orderInDb.OrderDate > DateTime.UtcNow.AddMinutes(-1));
                        
            Assert.NotNull(orderInDb.OrderItems);
            Assert.Equal(2, orderInDb.OrderItems.Count);
            
            var firstItem = orderInDb.OrderItems.FirstOrDefault(oi => oi.BookId == 1);
            Assert.NotNull(firstItem);
            Assert.Equal(2, firstItem.Quantity);
            Assert.Equal(10.00m, firstItem.UnitPrice);
        }
    }
}
