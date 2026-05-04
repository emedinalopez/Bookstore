using MediatR;
using InventoryService.Application.DTOs;

namespace InventoryService.Application.Categories.Queries
{    
    public class GetCategoryByIdQuery : IRequest<CategoryDTO?>
    {
        public int Id { get; set; }
    }
}
