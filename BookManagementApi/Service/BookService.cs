using BookManagementApi.Models;
using BookManagementApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementApi.Service
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        
        public async Task<List<string>> GetBookTitlesAsync(int page, int pageSize)
        {
            return await _bookRepository.GetBookTitlesAsync(page, pageSize);
        }

        public async  Task<BookDetailsDto> GetBookDetailsAsync(Guid id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                return null;
            }

            book.ViewsCount++;
            await _bookRepository.SaveAsync();

            var yearsSincePublished = DateTime.Now.Year - book.PublicationYear;
            var popularityScore = (book.ViewsCount * 0.5) + (yearsSincePublished * 2);

            return new BookDetailsDto
            {
                Book = book,
                PopularityScore = popularityScore
            };
        }

        public async Task<IActionResult> AddBookAsync(Book book)
        {
            if (string.IsNullOrEmpty(book.Title) || string.IsNullOrEmpty(book.AuthorName) || book.PublicationYear <= 0)
            {
                return new BadRequestObjectResult("Title, AuthorName, and PublicationYear are required and must be valid.");
            }
            
            if (await _bookRepository.BookExistsAsync(book.Title))
            {
                return new ConflictObjectResult("Book already exists.");
            }

            await _bookRepository.AddBookAsync(book);
            await _bookRepository.SaveAsync();
            return new CreatedAtActionResult("GetBookDetails", "Books", new { id = book.Id }, book);
        }

        public async Task<IActionResult> AddBooksAsync(List<Book> books)
        {
            if (books == null || !books.Any())
            {
                return new BadRequestObjectResult("No books to add. The list cannot be empty.");
            }
            
            foreach (var book in books)
            {
                if (string.IsNullOrEmpty(book.Title) || string.IsNullOrEmpty(book.AuthorName) || book.PublicationYear <= 0)
                {
                    return new BadRequestObjectResult("Each book must have a valid Title, AuthorName, and PublicationYear.");
                }
                
                if (await _bookRepository.BookExistsAsync(book.Title))
                {
                    return new ConflictObjectResult("Some books already exist.");
                }
            }

            await _bookRepository.AddBooksAsync(books);
            await _bookRepository.SaveAsync();
            return new CreatedAtActionResult("GetBookTitles", "Books", null, null);
        }

        public async Task<IActionResult> UpdateBookAsync(Guid id, Book updatedBook)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                return new NotFoundObjectResult("Book not found.");
            }
            
            if (string.IsNullOrEmpty(updatedBook.Title) || string.IsNullOrEmpty(updatedBook.AuthorName) || updatedBook.PublicationYear <= 0)
            {
                return new BadRequestObjectResult("Title, AuthorName, and PublicationYear are required and must be valid.");
            }

            book.Title = updatedBook.Title;
            book.AuthorName = updatedBook.AuthorName;
            book.PublicationYear = updatedBook.PublicationYear;

            await _bookRepository.UpdateBookAsync(book);
            await _bookRepository.SaveAsync();

            return new OkObjectResult(book);
        }

        public async Task<IActionResult> SoftDeleteBookAsync(Guid id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                return new NotFoundObjectResult("Book not found.");
            }

            await _bookRepository.SoftDeleteBookAsync(book);
            await _bookRepository.SaveAsync();

            return new OkObjectResult("Book deleted.");
        }

        public async Task<IActionResult> SoftDeleteBooksAsync(List<Guid> bookIds)
        {
            if (bookIds == null || !bookIds.Any())
            {
                return new BadRequestObjectResult("No book IDs provided.");
            }
            
            var books = new List<Book>();
            foreach (var bookId in bookIds)
            {
                var book = await _bookRepository.GetBookByIdAsync(bookId);
                if (book != null)
                {
                    books.Add(book);
                }
            }

            if (!books.Any())
            {
                return new NotFoundObjectResult("No books found.");
            }

            foreach (var book in books)
            {
                await _bookRepository.SoftDeleteBookAsync(book);
            }

            await _bookRepository.SaveAsync();
            return new OkObjectResult("Books deleted.");
        }
    }
}