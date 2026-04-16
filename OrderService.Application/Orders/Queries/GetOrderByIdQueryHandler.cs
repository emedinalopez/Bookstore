using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;

namespace OrderService.Application.Orders.Queries
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDTO?>
    {
        private readonly IOrderDbContext _context;

        public GetOrderByIdQueryHandler(IOrderDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDTO?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                .Where(o => o.Id == request.Id)                
                .Select(o => new OrderDTO
                {
                    Id = o.Id,
                    CustomerName = o.CustomerName,
                    OrderDate = o.OrderDate,
                    Status = o.Status.ToString(),
                    TotalAmount = o.TotalAmount,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDTO
                    {
                        Id = oi.Id,
                        BookId = oi.BookId,
                        BookTitle = oi.BookTitle,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
                        
            return order;
        }
    }
}
