using EventBus.Messages;
using InventoryService.Application.Books.Commands;
using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace InventoryService.Application.UnitTests.Books.Commands
{
    public class UpdateBookCommandHandlerTests
    {
        private readonly Mock<IInventoryDbContext> _mockDbContext;
        private readonly Mock<IMessagePublisher> _mockPublisher;

        public UpdateBookCommandHandlerTests()
        {
            _mockDbContext = new Mock<IInventoryDbContext>();
            _mockPublisher = new Mock<IMessagePublisher>();
        }

        [Fact]
        public async Task Handle_GivenValidCommand_ShouldUpdateBookAndPublishEvent()
        {
            // ========
            // Arrange
            // ========
            
            var existingBook = new Book
            {
                Id = 1,
                Title = "Old Title",
                Author = "Old Author",
                Price = 10.00m,
                StockQty = 50,
                CategoryId = 1
            };

            // Set up the DbContext mock to find the existing book
            var books = new List<Book> { existingBook }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Book>>();
                        
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(books.Provider);
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(books.Expression);
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(books.ElementType);
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(() => books.GetEnumerator());

            _mockDbContext.Setup(c => c.Books).Returns(mockDbSet.Object);

            // Create the update command with the new details
            var command = new UpdateBookCommand
            {
                Id = 1,
                Title = "New Updated Title",
                Author = "New Author",
                Price = 15.50m,
                StockQty = 75,
                CategoryId = 2
            };

            // D. Instantiate the handler with the mocks
            var handler = new UpdateBookCommandHandler(_mockDbContext.Object, _mockPublisher.Object);

            // =======
            // Act
            // =======
            var result = await handler.Handle(command, CancellationToken.None);

            // =======
            // Assert
            // =======

            // Verify the properties of the original entity were updated correctly
            Assert.Equal("New Updated Title", existingBook.Title);
            Assert.Equal(75, existingBook.StockQty);
            Assert.Equal(15.50m, existingBook.Price);

            // Verify the returned DTO matches the new data
            Assert.NotNull(result);
            Assert.Equal("New Updated Title", result.Title);

            // Verify SaveChangesAsync was called
            _mockDbContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            // D. Verify the publisher was called with the correct stock quantity
            _mockPublisher.Verify(p => p.PublishAsync(It.Is<BookStockChangedIntegrationEvent>(
                e => e.BookId == 1 && e.NewStockQuantity == 75
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_GivenInvalidId_ShouldThrowException()
        {
            // =======
            // Arrange
            // =======

            // Set up the mock DbContext to find nothing (return an empty list)
            var books = new List<Book>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Book>>();

            mockDbSet.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(books.Provider);
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(books.Expression);
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(books.ElementType);
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(() => books.GetEnumerator());

            _mockDbContext.Setup(c => c.Books).Returns(mockDbSet.Object);

            // Create a command with an ID that doesn't exist
            var command = new UpdateBookCommand { Id = 99 };

            // Instantiate the handler
            var handler = new UpdateBookCommandHandler(_mockDbContext.Object, _mockPublisher.Object);

            // =============
            // Act & Assert
            // =============

            // Verify that calling Handle with a bad ID throws an exception
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Verify that if an exception was thrown, we did NOT try to save or publish
            _mockDbContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _mockPublisher.Verify(p => p.PublishAsync(It.IsAny<object>()), Times.Never);
        }
    }
}
