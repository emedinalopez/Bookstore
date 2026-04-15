using MediatR;
using OrderService.Application.DTOs;

namespace OrderService.Application.Orders.Commands
{    
    public class CreateOrderCommand : IRequest<OrderDTO>
    {        
        public string CustomerName { get; set; } = string.Empty;
        public List<OrderItemCommand> Items { get; set; } = new List<OrderItemCommand>();
        public class OrderItemCommand
        {
            public int BookId { get; set; }
            public string BookTitle { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }
    }
}
