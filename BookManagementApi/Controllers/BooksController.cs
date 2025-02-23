using BookManagementApi.Models;
using BookManagementApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }
        
        [HttpGet("titles")]
        public async Task<ActionResult> GetBookTitles(int page = 1, int pageSize = 10)
        {
            var titles = await _bookService.GetBookTitlesAsync(page, pageSize);
            
            return Ok(titles);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetBookDetails(Guid id)
        {
            var result = await _bookService.GetBookDetailsAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            
            return Ok(result);
        }
        
        [HttpPost("book")]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            var result = await _bookService.AddBookAsync(book);
            
            return result;
        }
        
        [HttpPost]
        public async Task<IActionResult> AddBooks([FromBody] List<Book> books)
        {
            var result = await _bookService.AddBooksAsync(books);
            
            return result;
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] Book updatedBook)
        {
            var result = await _bookService.UpdateBookAsync(id, updatedBook);
            
            return result;
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteBook(Guid id)
        {
            var result = await _bookService.SoftDeleteBookAsync(id);
            
            return result;
        }

        [HttpDelete]
        public async Task<IActionResult> SoftDeleteBooks([FromBody] List<Guid> bookIds)
        {
            var result = await _bookService.SoftDeleteBooksAsync(bookIds);
            
            return result;
        }
    }
}