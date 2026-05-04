using MediatR;
using InventoryService.Application.DTOs;

namespace InventoryService.Application.Categories.Commands
{
    public class UpdateCategoryCommand : IRequest<CategoryDTO>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
