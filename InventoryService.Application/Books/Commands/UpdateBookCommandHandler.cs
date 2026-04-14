using InventoryService.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Application.Books.Commands
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand>
    {
        private readonly IInventoryDbContext _context;

        public UpdateBookCommandHandler(IInventoryDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var bookEntity = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (bookEntity == null)
            {                
                throw new Exception($"Book with ID {request.Id} not found.");
            }
            
            bookEntity.Title = request.Title;
            bookEntity.Author = request.Author;
            bookEntity.ISBN = request.ISBN;
            bookEntity.Price = request.Price;
            bookEntity.StockQty = request.StockQty;
            bookEntity.CategoryId = request.CategoryId;

            // _context.Books.Update(bookEntity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
