using Xunit;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Orders.Commands;
using OrderService.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Application.UnitTests.Orders.Commands
{
    public class DeleteOrderCommandHandlerTests
    {
        private OrderDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new OrderDbContext(options);
        }

        [Fact]
        public async Task Handle_GivenValidId_ShouldDeleteOrderAndItsItems()
        {
            // Arrange
            await using var dbContext = GetInMemoryDbContext();
            
            var orderToDelete = new Order
            {
                CustomerName = "Customer to Delete",
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { BookId = 1, Quantity = 1 },
                    new OrderItem { BookId = 2, Quantity = 5 }
                }
            };
            await dbContext.Orders.AddAsync(orderToDelete);
            await dbContext.SaveChangesAsync();
            
            var orderId = orderToDelete.Id;
            var command = new DeleteOrderCommand { Id = orderId };            
            var handler = new DeleteOrderCommandHandler(dbContext);

            // Acct
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var orderInDb = await dbContext.Orders.FindAsync(orderId);
            Assert.Null(orderInDb);
            
            var itemsInDb = await dbContext.OrderItems.AnyAsync(oi => oi.OrderId == orderId);
            Assert.False(itemsInDb);
        }

        [Fact]
        public async Task Handle_GivenInvalidId_ShouldDoNothing()
        {
            // Arrange
            await using var dbContext = GetInMemoryDbContext();            
            var command = new DeleteOrderCommand { Id = 99 };            
            var handler = new DeleteOrderCommandHandler(dbContext);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var anyOrders = await dbContext.Orders.AnyAsync();
            Assert.False(anyOrders);
        }

    }
}
