using BookManagementApi.DataAccess;
using BookManagementApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _libraryDbContext;

        public BooksController(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }
        
        [HttpPost("book")]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            var existingBook = await _libraryDbContext.Books
                .FirstOrDefaultAsync(b => b.Title.Equals(book.Title, StringComparison.OrdinalIgnoreCase) && !b.IsDeleted);

            if (existingBook != null)
            {
                return Conflict(new { message = "Book already exists." });
            }
            
            await _libraryDbContext.Books.AddAsync(book);
            await _libraryDbContext.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetBookDetails), new { id = book.Id }, book);
        }
        
        [HttpPost("books")]
        public async Task<IActionResult> AddBooks([FromBody] List<Book> books)
        {
            var existingBooks = await _libraryDbContext.Books
                .Where(b => books.Select(book => book.Title).Contains(b.Title) && !b.IsDeleted)
                .ToListAsync();

            if (existingBooks.Any())
            {
                var existingTitles = existingBooks.Select(b => b.Title).ToList();
                return Conflict(new { message = "The following books already exist: ", existingTitles });
            }
            
            await _libraryDbContext.Books.AddRangeAsync(books);
            await _libraryDbContext.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetBookTitles), null);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] Book updatedBook)
        {
            var book = await _libraryDbContext.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            book.Title = updatedBook.Title;
            book.PublicationYear = updatedBook.PublicationYear;
            book.AuthorName = updatedBook.AuthorName;
            await _libraryDbContext.SaveChangesAsync();

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteBook(Guid id)
        {
            var book = await _libraryDbContext.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            book.IsDeleted = true;
            await _libraryDbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("books")]
        public async Task<IActionResult> SoftDeleteBooks([FromBody] List<Guid> bookIds)
        {
            var books = await _libraryDbContext.Books
                .Where(b => bookIds.Contains(b.Id) && !b.IsDeleted)
                .ToListAsync();

            if (!books.Any())
            {
                return NotFound();
            }

            foreach (var book in books)
            {
                book.IsDeleted = true;
            }

            await _libraryDbContext.SaveChangesAsync();
            return NoContent();
        }
        
        [HttpGet("titles")]
        public async Task<ActionResult> GetBookTitles(int page = 1, int pageSize = 10)
        {
            var books = await _libraryDbContext.Books
                .Where(b => !b.IsDeleted)
                .OrderByDescending(b => b.ViewsCount)
                .Select(b => b.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(books);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetBookDetails(Guid id)
        {
            var book = await _libraryDbContext.Books.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
            if (book == null)
            {
                return NotFound();
            }

            book.ViewsCount++;
            await _libraryDbContext.SaveChangesAsync();

            var yearsSincePublished = DateTime.Now.Year - book.PublicationYear;
            var popularityScore = (book.ViewsCount * 0.5) + (yearsSincePublished * 2);

            return Ok(new { book, PopularityScore = popularityScore });
        }
    }
}