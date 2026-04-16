using MediatR;
using OrderService.Application.DTOs;

namespace OrderService.Application.Orders.Queries
{
    public class GetOrderByIdQuery : IRequest<OrderDTO?>
    {
        public int Id { get; set; }
    }
}
