using Xunit;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Orders.Queries;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Application.UnitTests.Orders.Queries
{
    public class GetAllOrdersQueryHandlerTests
    {        
        private OrderDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new OrderDbContext(options);
        }

        [Fact]
        public async Task Handle_WhenOrdersExist_ShouldReturnListOfOrderDtos()
        {
            // Arrange
            await using var dbContext = GetInMemoryDbContext();
            var ordersToSeed = new List<Order>
            {
                new Order { CustomerName = "Customer A", Status = OrderStatus.Pending },
                new Order { CustomerName = "Customer B", Status = OrderStatus.Processing, OrderItems = new List<OrderItem> { new OrderItem() } }
            };
            await dbContext.Orders.AddRangeAsync(ordersToSeed);
            await dbContext.SaveChangesAsync();
            
            var query = new GetAllOrdersQuery();
            var handler = new GetAllOrdersQueryHandler(dbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
                        
            var customerBOrder = result.FirstOrDefault(o => o.CustomerName == "Customer B");
            Assert.NotNull(customerBOrder);
            Assert.Equal(OrderStatus.Processing.ToString(), customerBOrder.Status);
            
            Assert.Single(customerBOrder.OrderItems);
        }

        [Fact]
        public async Task Handle_WhenNoOrdersExist_ShouldReturnEmptyList()
        {
            // Arrange
            await using var dbContext = GetInMemoryDbContext();            
            var query = new GetAllOrdersQuery();
            var handler = new GetAllOrdersQueryHandler(dbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
