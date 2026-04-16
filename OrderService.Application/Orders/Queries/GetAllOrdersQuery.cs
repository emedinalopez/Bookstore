using MediatR;
using OrderService.Application.DTOs;

namespace OrderService.Application.Orders.Queries
{    
    public class GetAllOrdersQuery : IRequest<IEnumerable<OrderDTO>>
    {        
    }
}
