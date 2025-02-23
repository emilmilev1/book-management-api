using BookManagementApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementApi.Service
{
    public interface IBookService
    {
        Task<List<string>> GetBookTitlesAsync(int page, int pageSize);
        Task<BookDetailsDto> GetBookDetailsAsync(Guid id);
        Task<IActionResult> AddBookAsync(Book book);
        Task<IActionResult> AddBooksAsync(List<Book> books);
        Task<IActionResult> UpdateBookAsync(Guid id, Book updatedBook);
        Task<IActionResult> SoftDeleteBookAsync(Guid id);
        Task<IActionResult> SoftDeleteBooksAsync(List<Guid> bookIds);
    }
}