using MediatR;
using InventoryService.Application.DTOs;

namespace InventoryService.Application.Categories.Commands
{
    public class CreateCategoryCommand : IRequest<CategoryDTO>
    {
        public string Name { get; set; }
    }
}
