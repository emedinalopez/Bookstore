using InventoryService.Application.Books.Queries;
using InventoryService.Domain.Entities;
using InventoryService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Application.UnitTests.Books.Queries
{
    public class GetBookByIdQueryHandlerTests
    {        
        private InventoryDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<InventoryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new InventoryDbContext(options);
        }

        [Fact]
        public async Task Handle_GivenValidId_ShouldReturnCorrectBookDto()
        {            
            // Arrange  
            await using var dbContext = GetInMemoryDbContext();
            
            var bookToFind = new Book
            {
                Title = "The Hobbit",
                Author = "J.R.R. Tolkien",
                Price = 19.99m
            };
            dbContext.Books.Add(bookToFind);
            await dbContext.SaveChangesAsync(); 
            
            var bookId = bookToFind.Id;
            var query = new GetBookByIdQuery { BookId = bookId };            
            var handler = new GetBookByIdQueryHandler(dbContext);
                        
            // Act            
            var result = await handler.Handle(query, CancellationToken.None);
                        
            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookId, result.ID);
            Assert.Equal("The Hobbit", result.Title);
            Assert.Equal(19.99m, result.Price);
        }

        [Fact]
        public async Task Handle_GivenInvalidId_ShouldReturnNull()
        {
            
            // Arrange
            await using var dbContext = GetInMemoryDbContext();            
            var query = new GetBookByIdQuery { BookId = 99 };            
            var handler = new GetBookByIdQueryHandler(dbContext);
                        
            // Act            
            var result = await handler.Handle(query, CancellationToken.None);
                        
            // Assert
            Assert.Null(result);
        }
    }
}
