using InventoryService.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InventoryService.Application.Books.Queries;
using MediatR;

namespace InventoryService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAllBooks()
        {
            var books = await _mediator.Send(new GetAllBooksQuery());
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBookById(int id)
        {
            var book = await _mediator.Send(new GetBooksByIdQuery { GetBookById = id });
            return book != null ? Ok(book) : NotFound();
        }

        //POST: api/book
        [HttpPost]
        public async Task<ActionResult<BookDTO>> CreateBook([FromBody] CreateBookCommand command)
        {
            var createdBook = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
        }

        //PUT: api/book/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Id does not match in route vs body");
            }

            await _mediator.Send(command);
            return NoContent();
        }

        //DELETE: api/book/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _mediator.Send(new DeleteBookCommand { Id = id });
            return NoContent();
        }
    }
}
