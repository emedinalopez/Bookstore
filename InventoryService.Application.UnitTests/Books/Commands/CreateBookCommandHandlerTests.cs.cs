using EventBus.Messages;
using InventoryService.Application.Books.Commands;
using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace InventoryService.Application.UnitTests.Books.Commands
{
    public class CreateBookCommandHandlerTests
    {        
        [Fact]
        public async Task Handle_GivenValidCommand_ShouldCreateBookAndPublishEvent()
        {            
            // Arrange            
            var dbOptions = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(databaseName: $"InventoryDb_{System.Guid.NewGuid()}")
                .Options;

            var mockDbContext = new Mock<IInventoryDbContext>();
            var mockDbSet = new Mock<DbSet<Book>>();

            mockDbContext.Setup(c => c.Books).Returns(mockDbSet.Object);
                        
            var mockPublisher = new Mock<IMessagePublisher>();            
            var command = new CreateBookCommand
            {
                Title = "The Lord of the Rings",
                Author = "J.R.R. Tolkien",
                Price = 29.99m,
                StockQty = 100,
                CategoryId = 1
            };
            
            var handler = new CreateBookCommandHandler(mockDbContext.Object, mockPublisher.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            
            // Assert
            // Verify the handler returned a valid DTO
            Assert.NotNull(result);
            Assert.Equal("The Lord of the Rings", result.Title);
            Assert.Equal(100, result.StockQty);

            // Verify the database Add method was called exactly once
            mockDbSet.Verify(dbSet => dbSet.Add(It.IsAny<Book>()), Times.Once);

            // Verify SaveChangesAsync was called exactly once
            mockDbContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            // Verify the Publisher was called exactly once with the correct event type
            mockPublisher.Verify(p => p.PublishAsync(It.Is<BookStockChangedIntegrationEvent>(e => e.NewStockQuantity == 100)), Times.Once);
        }
    }
}
