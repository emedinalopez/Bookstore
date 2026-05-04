using MediatR;

namespace InventoryService.Application.Categories.Commands
{    
    public class DeleteCategoryCommand : IRequest
    {
        public int Id { get; set; }
    }
}
