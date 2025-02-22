namespace BookManagementApi.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PublicationYear { get; set; }
        public string AuthorName { get; set; }
        public int ViewsCout { get; set; }
        public bool IsDeleted { get; set; }
    }
}