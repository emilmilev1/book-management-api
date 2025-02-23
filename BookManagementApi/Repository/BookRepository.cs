using BookManagementApi.DataAccess;
using BookManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagementApi.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _libraryDbContext;

        public BookRepository(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }
        
        public async Task<List<string>> GetBookTitlesAsync(int page, int pageSize)
        {
            return await _libraryDbContext.Books
                .Where(b => !b.IsDeleted)
                .OrderByDescending(b => b.ViewsCount)
                .Select(b => b.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(Guid id)
        {
            return (await _libraryDbContext.Books.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted))!;
        }

        public async Task<bool> BookExistsAsync(string title)
        {
            return await _libraryDbContext.Books.AnyAsync(b => b.Title.ToLower() == title.ToLower() && !b.IsDeleted);
        }

        public async Task AddBookAsync(Book book)
        {
            await _libraryDbContext.Books.AddAsync(book);
        }

        public async Task AddBooksAsync(List<Book> books)
        {
            await _libraryDbContext.Books.AddRangeAsync(books);
        }

        public Task UpdateBookAsync(Book book)
        {
            _libraryDbContext.Books.Update(book);
            return Task.CompletedTask;
        }

        public Task SoftDeleteBookAsync(Book book)
        {
            book.IsDeleted = true;
            _libraryDbContext.Books.Update(book);
            return Task.CompletedTask;
        }
        
        public async Task SaveAsync()
        {
            await _libraryDbContext.SaveChangesAsync();
        }
    }
}