using EventBus.Messages;
using InventoryService.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Application.Books.Commands
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
    {
        private readonly IInventoryDbContext _context;
        private readonly IMessagePublisher _publisher;

        public DeleteBookCommandHandler(IInventoryDbContext context, IMessagePublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var bookEntity = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (bookEntity == null)
            {
                //TODO: Log this warning and avoid execution of next tasks
                Console.WriteLine($"Book not found.");                
            }

            _context.Books.Remove(bookEntity);
            await _context.SaveChangesAsync(cancellationToken);

            var integrationEvent = new BookStockChangedIntegrationEvent
            {
                BookId = bookEntity.Id,
                NewStockQuantity = 0
            };

            await _publisher.PublishAsync(integrationEvent);
        }
    }
}