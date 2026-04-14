using MediatR;
using InventoryService.Application.DTOs;

namespace InventoryService.Application.Categories.Queries
{    
    public class GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDTO>>
    {        
    }
}
