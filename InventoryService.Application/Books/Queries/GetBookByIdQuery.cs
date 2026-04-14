using InventoryService.Application.DTOs;
using MediatR;

namespace InventoryService.Application.Books.Queries
{
    public class GetBookByIdQuery : IRequest<BookDTO?>
    {
        public int BookId { get; set; }
    }
}
