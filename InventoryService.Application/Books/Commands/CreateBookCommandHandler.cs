using InventoryService.Application.DTOs;
using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;
using MediatR;

namespace InventoryService.Application.Books.Commands
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookDTO>
    {
        private readonly IInventoryDbContext _context;

        public CreateBookCommandHandler(IInventoryDbContext context)
        {
            _context = context;
        }

        public async Task<BookDTO> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var bookEntity = new Book
            {
                Title = request.Title,
                Author = request.Author,
                ISBN = request.ISBN,
                Price = request.Price,
                StockQty = request.StockQty,
                CategoryId = request.CategoryId
            };

            _context.Books.Add(bookEntity);
            await _context.SaveChangesAsync(cancellationToken);

            return new BookDTO
            {
                ID = bookEntity.Id,
                Title = bookEntity.Title,
                Author = bookEntity.Author,
                ISBN = bookEntity.ISBN,
                Price= bookEntity.Price,
                StockQty = bookEntity.StockQty,
                CategoryId = bookEntity.CategoryId,
                CategoryName = ""
            };
        }
    }
}
