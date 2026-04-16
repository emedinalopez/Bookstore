using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Domain.Enums;

namespace OrderService.Application.Orders.Commands
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderDbContext _context;

        public UpdateOrderCommandHandler(IOrderDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {            
            var orderEntity = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (orderEntity == null)
            {                
                throw new Exception($"Order with ID {request.Id} not found.");
            }
                        
            if (!Enum.TryParse(request.NewStatus, true, out OrderStatus newStatus))
            {
                throw new ArgumentException($"Invalid status value: '{request.NewStatus}'.");
            }
                        
            orderEntity.Status = newStatus;
            
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
