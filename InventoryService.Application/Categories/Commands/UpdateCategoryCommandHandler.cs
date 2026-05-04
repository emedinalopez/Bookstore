using Bookstore.Common.Exceptions;
using InventoryService.Application.DTOs;
using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;
using MediatR;

namespace InventoryService.Application.Categories.Commands
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDTO>
    {
        private readonly IInventoryDbContext _context;

        public UpdateCategoryCommandHandler(IInventoryDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryDTO> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Category.FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {                
                throw new NotFoundException(nameof(Category), request.Id);
            }

            entity.Name = request.Name;

            await _context.SaveChangesAsync(cancellationToken);

            return new CategoryDTO
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
