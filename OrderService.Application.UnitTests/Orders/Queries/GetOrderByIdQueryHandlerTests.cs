using Microsoft.EntityFrameworkCore;
using OrderService.Application.Orders.Queries;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Application.UnitTests.Orders.Queries
{
    public class GetOrderByIdQueryHandlerTests
    {
        // Helper method to create a fresh In-Memory DbContext for each test
        private OrderDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new OrderDbContext(options);
        }

        [Fact]
        public async Task Handle_GivenValidId_ShouldReturnCorrectOrderDto()
        {
            // Arrange
            await using var dbContext = GetInMemoryDbContext();            
            var orderToFind = new Order
            {
                CustomerName = "Test Customer",
                Status = OrderStatus.Pending,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { BookId = 1, Quantity = 1, UnitPrice = 10m },
                    new OrderItem { BookId = 2, Quantity = 2, UnitPrice = 20m }
                }
            };
            await dbContext.Orders.AddAsync(orderToFind);
            await dbContext.SaveChangesAsync();
            
            var orderId = orderToFind.Id;
            var query = new GetOrderByIdQuery { Id = orderId };            
            var handler = new GetOrderByIdQueryHandler(dbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.Id);
            Assert.Equal("Test Customer", result.CustomerName);
            Assert.Equal(OrderStatus.Pending.ToString(), result.Status);

            // B. Verify the order items were also fetched and mapped
            Assert.NotNull(result.OrderItems);
            Assert.Equal(2, result.OrderItems.Count);
            Assert.True(result.OrderItems.Any(oi => oi.BookId == 2 && oi.Quantity == 2));
        }

        [Fact]
        public async Task Handle_GivenInvalidId_ShouldReturnNull()
        {
            // Arrange
            await using var dbContext = GetInMemoryDbContext();            
            var query = new GetOrderByIdQuery { Id = 99 };
            var handler = new GetOrderByIdQueryHandler(dbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
