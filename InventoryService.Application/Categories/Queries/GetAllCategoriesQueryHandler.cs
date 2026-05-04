using MediatR;
using Microsoft.EntityFrameworkCore;
using InventoryService.Application.DTOs;
using InventoryService.Application.Interfaces;

namespace InventoryService.Application.Categories.Queries
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDTO>>
    {
        private readonly IInventoryDbContext _context;

        public GetAllCategoriesQueryHandler(IInventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDTO>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _context.Category
                .AsNoTracking() 
                .OrderBy(c => c.Name) 
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync(cancellationToken);

            return categories;
        }
    }
}
