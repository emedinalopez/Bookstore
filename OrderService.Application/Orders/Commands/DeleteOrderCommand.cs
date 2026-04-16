using MediatR;

namespace OrderService.Application.Orders.Commands
{    
    public class DeleteOrderCommand : IRequest
    {
        public int Id { get; set; }
    }
}
