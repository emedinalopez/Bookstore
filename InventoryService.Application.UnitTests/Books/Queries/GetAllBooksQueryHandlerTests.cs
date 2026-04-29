using InventoryService.Application.Books.Queries;
using InventoryService.Domain.Entities;
using InventoryService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Application.UnitTests.Books.Queries
{
    public class GetAllBooksQueryHandlerTests
    {        
        private InventoryDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<InventoryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new InventoryDbContext(options);
        }

        [Fact]
        public async Task Handle_WhenBooksExist_ShouldReturnListOfBookDtos()
        {
            // Arrange
            await using var dbContext = GetInMemoryDbContext();
            
            var booksToSeed = new List<Book>
            {
                new Book { Title = "Book One", Author = "Author A", Price = 10.00m },
                new Book { Title = "Book Two", Author = "Author B", Price = 20.00m },
                new Book { Title = "Book Three", Author = "Author C", Price = 30.00m }
            };

            await dbContext.Books.AddRangeAsync(booksToSeed);
            await dbContext.SaveChangesAsync();
            
            var query = new GetAllBooksQuery();
            var handler = new GetAllBooksQueryHandler(dbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            
            var firstBookDto = result.FirstOrDefault(b => b.Title == "Book One");
            Assert.NotNull(firstBookDto);
            Assert.Equal("Author A", firstBookDto.Author);
            Assert.Equal(10.00m, firstBookDto.Price);
        }

        [Fact]
        public async Task Handle_WhenNoBooksExist_ShouldReturnEmptyList()
        {
            // Arrange
            await using var dbContext = GetInMemoryDbContext();
            
            var query = new GetAllBooksQuery();
            var handler = new GetAllBooksQueryHandler(dbContext);
                        
            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
