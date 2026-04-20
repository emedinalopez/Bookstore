using InventoryService.Application.DTOs;
using InventoryService.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Application.Books.Queries
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, IEnumerable<BookDTO>>
    {
        private readonly IInventoryDbContext _context;

        public GetAllBooksQueryHandler(IInventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookDTO>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _context.Books
                .AsNoTracking()
                .Include(books => books.Category)
                .Select(books => new BookDTO
                {
                    ID = books.Id,
                    Title = books.Title,
                    Author = books.Author,
                    ISBN = books.ISBN,
                    Price = books.Price,
                    StockQty = books.StockQty,
                    CategoryId = books.CategoryId
                    //CategoryName = books.CategoryName
                })
                .ToListAsync(cancellationToken);

            return books;
        }
    }
}
