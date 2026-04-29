using EventBus.Messages;
using InventoryService.Application.Books.Commands;
using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;
using InventoryService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace InventoryService.Application.UnitTests.Books.Commands
{
    public class UpdateBookCommandHandlerTests
    {
        private readonly Mock<IMessagePublisher> _mockPublisher;

        public UpdateBookCommandHandlerTests()
        {
            _mockPublisher = new Mock<IMessagePublisher>();
        }
        
        private InventoryDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<InventoryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new InventoryDbContext(options);
        }

        [Fact]
        public async Task Handle_GivenValidCommand_ShouldUpdateBookAndPublishEvent()
        {
            // ========
            // Arrange
            // ========
                        
            using var dbContext = GetInMemoryDbContext();
                        
            var existingBook = new Book
            {
                Title = "Old Title",
                Author = "Old Author",
                Price = 10.00m,
                StockQty = 50,
                CategoryId = 1
            };

            dbContext.Books.Add(existingBook);
            await dbContext.SaveChangesAsync();
                        
            var bookIdToUpdate = existingBook.Id;
            
            var command = new UpdateBookCommand
            {
                Id = bookIdToUpdate,
                Title = "New Updated Title",
                Author = "New Author",
                Price = 15.50m,
                StockQty = 75,
                CategoryId = 2
            };
                        
            var handler = new UpdateBookCommandHandler(dbContext, _mockPublisher.Object);

            // ========
            // Act
            // ========
            var result = await handler.Handle(command, CancellationToken.None);

            // ========
            // Assert
            // ========

            // Verify the database was actually updated
            var updatedBookInDb = await dbContext.Books.FindAsync(bookIdToUpdate);
            Assert.NotNull(updatedBookInDb);
            Assert.Equal("New Updated Title", updatedBookInDb.Title);
            Assert.Equal(75, updatedBookInDb.StockQty);
            Assert.Equal(15.50m, updatedBookInDb.Price);

            // Check the returned DTO matches the new data
            Assert.NotNull(result);
            Assert.Equal("New Updated Title", result.Title);
            Assert.Equal(75, result.StockQty);

            // Check the publisher was called exactly once with the correct new stock quantity
            _mockPublisher.Verify(p => p.PublishAsync(It.Is<BookStockChangedIntegrationEvent>(
                e => e.BookId == bookIdToUpdate && e.NewStockQuantity == 75
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_GivenInvalidId_ShouldThrowException()
        {
            // =========
            // Arrange
            // =========                        
            using var dbContext = GetInMemoryDbContext();            
            
            var command = new UpdateBookCommand { Id = 99 };            
            var handler = new UpdateBookCommandHandler(dbContext, _mockPublisher.Object);

            // ==============
            // Act & Assert
            // ==============

            // Verify that calling Handle throws an exception
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Verify the publisher was not called because the exception was thrown
            _mockPublisher.Verify(p => p.PublishAsync(It.IsAny<object>()), Times.Never);
        }
    }
}
