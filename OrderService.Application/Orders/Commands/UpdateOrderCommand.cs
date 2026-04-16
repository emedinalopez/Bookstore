using MediatR;

namespace OrderService.Application.Orders.Commands
{
    public class UpdateOrderCommand : IRequest
    { 
        public int Id { get; set; }
        public string NewStatus { get; set; } = string.Empty;
    }
}
