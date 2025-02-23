using BookManagementApi.Models;

namespace BookManagementApi.Repository
{
    public interface IBookRepository
    {
        Task<List<string>> GetBookTitlesAsync(int page, int pageSize);
        Task<Book> GetBookByIdAsync(Guid id);
        Task<bool> BookExistsAsync(string title);
        Task AddBookAsync(Book book);
        Task AddBooksAsync(List<Book> books);
        Task UpdateBookAsync(Book book);
        Task SoftDeleteBookAsync(Book book);
        Task SaveAsync();
    }
}