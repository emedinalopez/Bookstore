using MediatR;

namespace InventoryService.Application.Books.Commands
{
    public class DeleteBookCommand : IRequest
    {
        public int Id { get; set; }
    }
}
