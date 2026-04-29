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

// Note: You may need to add a using statement for where your actual DbContext lives
using InventoryService.Infrastructure.Persistence;

namespace InventoryService.Application.UnitTests.Books.Commands
{
    public class UpdateBookCommandHandlerTests
    {
        private readonly Mock<IMessagePublisher> _mockPublisher;

        public UpdateBookCommandHandlerTests()
        {
            _mockPublisher = new Mock<IMessagePublisher>();
        }

        // Helper method to create a fresh, empty In-Memory database for each test
        private InventoryDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<InventoryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name per test
                .Options;

            return new InventoryDbContext(options);
        }

        [Fact]
        public async Task Handle_GivenValidCommand_ShouldUpdateBookAndPublishEvent()
        {
            // ==========================================
            // 1. ARRANGE
            // ==========================================

            // A. Get a fresh In-Memory database
            using var dbContext = GetInMemoryDbContext();

            // B. Seed the database with an existing book
            var existingBook = new Book
            {
                Title = "Old Title",
                Author = "Old Author",
                Price = 10.00m,
                StockQty = 50,
                CategoryId = 1
            };
            dbContext.Books.Add(existingBook);
            await dbContext.SaveChangesAsync(); // Save the seed data!

            // Note: In-Memory DB auto-generates IDs, so existingBook.Id will be 1
            var bookIdToUpdate = existingBook.Id;

            // C. Create the update command with the new details
            var command = new UpdateBookCommand
            {
                Id = bookIdToUpdate,
                Title = "New Updated Title",
                Author = "New Author",
                Price = 15.50m,
                StockQty = 75,
                CategoryId = 2
            };

            // D. Instantiate the handler
            var handler = new UpdateBookCommandHandler(dbContext, _mockPublisher.Object);

            // ==========================================
            // 2. ACT
            // ==========================================
            var result = await handler.Handle(command, CancellationToken.None);

            // ==========================================
            // 3. ASSERT
            // ==========================================

            // A. Verify the database was actually updated
            var updatedBookInDb = await dbContext.Books.FindAsync(bookIdToUpdate);
            Assert.NotNull(updatedBookInDb);
            Assert.Equal("New Updated Title", updatedBookInDb.Title);
            Assert.Equal(75, updatedBookInDb.StockQty);
            Assert.Equal(15.50m, updatedBookInDb.Price);

            // B. Verify the returned DTO matches the new data
            Assert.NotNull(result);
            Assert.Equal("New Updated Title", result.Title);
            Assert.Equal(75, result.StockQty);

            // C. Verify the publisher was called exactly once with the correct new stock quantity
            _mockPublisher.Verify(p => p.PublishAsync(It.Is<BookStockChangedIntegrationEvent>(
                e => e.BookId == bookIdToUpdate && e.NewStockQuantity == 75
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_GivenInvalidId_ShouldThrowException()
        {
            // ==========================================
            // 1. ARRANGE
            // ==========================================

            // A. Get a fresh In-Memory database (it is completely empty)
            using var dbContext = GetInMemoryDbContext();

            // B. Create a command with an ID that doesn't exist in the empty DB
            var command = new UpdateBookCommand { Id = 99 };

            // C. Instantiate the handler
            var handler = new UpdateBookCommandHandler(dbContext, _mockPublisher.Object);

            // ==========================================
            // 2. ACT & 3. ASSERT
            // ==========================================

            // Verify that calling Handle throws an exception (because it won't find ID 99)
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Verify the publisher was NEVER called since it failed early
            _mockPublisher.Verify(p => p.PublishAsync(It.IsAny<object>()), Times.Never);
        }
    }
}
