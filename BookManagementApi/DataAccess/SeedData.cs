using BookManagementApi.Models;

namespace BookManagementApi.DataAccess
{
    public static class SeedData
    {
        public static void CreateLibrary(LibraryDbContext libraryDbContext)
        {
            if (libraryDbContext.Books.Any())
            {
                return;
            }
            
            var random = new Random();
            var books = new List<Book>();

            for (var i = 0; i < 100; i++)
            {
                books.Add(new Book
                {
                    Id = Guid.NewGuid(),
                    Title = $"Book Title {i}",
                    AuthorName = $"Author {i}",
                    PublicationYear = random.Next(1900, DateTime.Now.Year + 1),
                    ViewsCount = random.Next(0, 500),
                    IsDeleted = false
                });
            }
            
            libraryDbContext.Books.AddRange(books);
            
            try
            {
                libraryDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while seeding data: {ex.Message}");
            }
        }
    }
}