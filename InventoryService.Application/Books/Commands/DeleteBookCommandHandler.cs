using InventoryService.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Application.Books.Commands
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
    {
        private readonly IInventoryDbContext _context;

        public DeleteBookCommandHandler(IInventoryDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var bookEntity = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (bookEntity == null)
            {
                //TODO: Log this warning and avoid execution of next task
                Console.WriteLine($"Book not found.");                
            }

            _context.Books.Remove(bookEntity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}