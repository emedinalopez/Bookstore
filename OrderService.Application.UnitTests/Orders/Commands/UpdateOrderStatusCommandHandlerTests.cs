using Microsoft.EntityFrameworkCore;
using OrderService.Application.Orders.Commands;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Application.UnitTests.Orders.Commands
{
    public class UpdateOrderStatusCommandHandlerTests
    {        
        private OrderDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new OrderDbContext(options);
        }

        [Fact]
        public async Task Handle_GivenValidCommand_ShouldUpdateOrderStatus()
        {
            // Arrange
            await using var dbContext = GetInMemoryDbContext();
            
            var existingOrder = new Order
            {
                CustomerName = "John Doe",
                Status = OrderStatus.Pending
            };
            await dbContext.Orders.AddAsync(existingOrder);
            await dbContext.SaveChangesAsync();
            
            var orderIdToUpdate = existingOrder.Id;            
            var command = new UpdateOrderCommand
            {
                Id = orderIdToUpdate,
                NewStatus = OrderStatus.Processing.ToString()
            };
            
            var handler = new UpdateOrderCommandHandler(dbContext);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert            
            var updatedOrderInDb = await dbContext.Orders.FindAsync(orderIdToUpdate);
            
            Assert.NotNull(updatedOrderInDb);
            Assert.Equal(OrderStatus.Processing, updatedOrderInDb.Status);
        }

        [Fact]
        public async Task Handle_GivenInvalidId_ShouldThrowException()
        {
            // Arrange
            await using var dbContext = GetInMemoryDbContext();
            
            var command = new UpdateOrderCommand { Id = 99, NewStatus = OrderStatus.Completed.ToString() };            
            var handler = new UpdateOrderCommandHandler(dbContext);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
