using BookManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagementApi.DataAccess
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) 
            : base(options)
        {
            
        }
        
        public DbSet<Book> Books { get; set; }
    }
}