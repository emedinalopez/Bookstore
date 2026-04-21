using InventoryService.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using InventoryService.Application.DTOs;
using EventBus.Messages;

namespace InventoryService.Application.Books.Commands
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookDTO>
    {
        private readonly IInventoryDbContext _context;
        private readonly IMessagePublisher _publisher;

        public UpdateBookCommandHandler(IInventoryDbContext context, IMessagePublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task<BookDTO> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
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

            var integrationEvent = new BookStockChangedIntegrationEvent
            {
                BookId = bookEntity.Id,
                NewStockQuantity = bookEntity.StockQty
            };

            await _publisher.PublishAsync(integrationEvent);

            return new BookDTO
            {
                ID = bookEntity.Id,
                Title = bookEntity.Title,
                Author = bookEntity.Author,
                ISBN = bookEntity.ISBN,
                Price = bookEntity.Price,
                StockQty = bookEntity.StockQty,
                CategoryId = bookEntity.CategoryId
            };
        }
    }
}
