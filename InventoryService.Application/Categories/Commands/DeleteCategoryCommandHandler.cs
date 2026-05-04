using Bookstore.Common.Exceptions;
using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Application.Categories.Commands
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IInventoryDbContext _context;

        public DeleteCategoryCommandHandler(IInventoryDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Category.FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }
            
            var hasBooks = await _context.Books.AnyAsync(b => b.CategoryId == request.Id, cancellationToken);
            if (hasBooks)
            {                
                throw new ValidationException("This category cannot be deleted because it is assigned to one or more books.");
            }

            _context.Category.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
