using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;

namespace OrderService.Application.Orders.Commands
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderDbContext _context;

        public DeleteOrderCommandHandler(IOrderDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {            
            var orderEntity = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
                        
            if (orderEntity == null)
            {                
                return;
            }
                        
            _context.Orders.Remove(orderEntity);
            
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
