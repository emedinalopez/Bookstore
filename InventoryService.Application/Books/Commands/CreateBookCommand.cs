using InventoryService.Application.DTOs;
using MediatR;

namespace InventoryService.Application.Books.Commands
{
    public class CreateBookCommand : IRequest<BookDTO>
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQty { get; set; }
        public int CategoryId { get; set; }
    }
}
