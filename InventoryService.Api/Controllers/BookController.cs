using InventoryService.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAllBooks()
        {
            var books = new List<BookDTO>();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBookById(int id)
        {
            var book = new BookDTO();
            return Ok(book);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookDTO>> CreateBook(int id, [FromBody] BookDTO book)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBook([FromBody] BookDTO book)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            throw new NotImplementedException();
        }
    }
}
