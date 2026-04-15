using MediatR;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;

namespace OrderService.Application.Orders.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDTO>
    {
        private readonly IOrderDbContext _context;

        public CreateOrderCommandHandler(IOrderDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDTO> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {            
            var orderEntity = new Order
            {
                CustomerName = request.CustomerName,
                OrderDate = DateTime.UtcNow, 
                Status = OrderStatus.Pending
            };
            
            foreach (var item in request.Items)
            {
                var orderItem = new OrderItem
                {
                    BookId = item.BookId,
                    BookTitle = item.BookTitle,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Order = orderEntity
                };
                orderEntity.OrderItems.Add(orderItem);
            }
            
            orderEntity.TotalAmount = orderEntity.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity);
            
            _context.Orders.Add(orderEntity);
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return new OrderDTO
            {
                Id = orderEntity.Id,
                CustomerName = orderEntity.CustomerName,
                OrderDate = orderEntity.OrderDate,
                Status = orderEntity.Status.ToString(),
                TotalAmount = orderEntity.TotalAmount,
                OrderItems = orderEntity.OrderItems.Select(oi => new OrderItemDTO
                {
                    Id = oi.Id,
                    BookId = oi.BookId,
                    BookTitle = oi.BookTitle,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };
        }
    }
}
