using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using InventoryService.Application.Books.Commands;
using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;
using EventBus.Messages;
using System.Threading;
using System.Threading.Tasks;
using System;
using InventoryService.Infrastructure.Persistence;

namespace InventoryService.Application.UnitTests.Books.Commands
{
    public class DeleteBookCommandHandlerTests
    {
        private readonly Mock<IMessagePublisher> _mockPublisher;

        public DeleteBookCommandHandlerTests()
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
        public async Task Handle_GivenValidId_ShouldDeleteBookAndPublishEvent()
        {
            // =========
            // Arrange
            // =========
                        
            await using var dbContext = GetInMemoryDbContext();
            
            var bookToDelete = new Book
            {
                Title = "Book to be Deleted",
                Author = "Test Author",
                Price = 9.99m,
                StockQty = 10,
                CategoryId = 1
            };
            dbContext.Books.Add(bookToDelete);
            await dbContext.SaveChangesAsync();
            
            var bookId = bookToDelete.Id;
            var command = new DeleteBookCommand { Id = bookId };            
            var handler = new DeleteBookCommandHandler(dbContext, _mockPublisher.Object);

            // ======
            // Act
            // ======
            await handler.Handle(command, CancellationToken.None);

            // ========
            // Assert
            // ========
            // 
            var bookInDb = await dbContext.Books.FindAsync(bookId);
            Assert.Null(bookInDb);
            
            _mockPublisher.Verify(p => p.PublishAsync(It.Is<BookStockChangedIntegrationEvent>(
                e => e.BookId == bookId && e.NewStockQuantity == 0
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_GivenInvalidId_ShouldDoNothing()
        {
            // =========
            // Arrange
            // =========
                        
            await using var dbContext = GetInMemoryDbContext();            
            var command = new DeleteBookCommand { Id = 99 };
            var handler = new DeleteBookCommandHandler(dbContext, _mockPublisher.Object);

            // ======
            // Act
            // ======
            
            await handler.Handle(command, CancellationToken.None);

            // ========
            // Assert
            // ========
                        
            _mockPublisher.Verify(p => p.PublishAsync(It.IsAny<object>()), Times.Never);
        }
    }
}
