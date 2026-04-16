using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Orders.Commands;
using OrderService.Application.DTOs;

namespace OrderService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var createdOrder = await _mediator.Send(command);
                        
            return Ok(createdOrder);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderCommand command)
        {            
            if (command == null)
            {
                return BadRequest();
            }
            command.Id = id;

            await _mediator.Send(command);
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var command = new DeleteOrderCommand { Id = id };

            await _mediator.Send(command);
            
            return NoContent();
        }
    }
}
