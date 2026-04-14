using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using InventoryService.Application.DTOs;
using InventoryService.Application.Interfaces;

namespace InventoryService.Application.Books.Queries
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDTO?>
    {
        private readonly IInventoryDbContext _context;

        public GetBookByIdQueryHandler(IInventoryDbContext context)
        {
            _context = context;
        }

        public async Task<BookDTO?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await _context.Books
                .AsNoTracking()
                .Include(b => b.Category)
                .Where(b => b.Id == request.BookId)
                .Select(b => new BookDTO
                {
                    ID = request.BookId,
                    Title = b.Title,
                    Author = b.Author,
                    ISBN = b.ISBN,
                    Price = b.Price,
                    StockQty = b.StockQty,
                    CategoryId = b.CategoryId                  
                })
                .FirstOrDefaultAsync(cancellationToken);

            return book;
        }
    }
}
