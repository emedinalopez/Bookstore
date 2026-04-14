using InventoryService.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using InventoryService.Application.Categories.Queries;

namespace InventoryService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //GET: api/category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDTO categoryDTO)
        {  
            throw new NotImplementedException(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            throw new NotImplementedException();
        }
    }
}
