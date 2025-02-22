using System.ComponentModel.DataAnnotations;
using BookManagementApi.Common;

namespace BookManagementApi.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(Constants.BookTitleMaxLength)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public int PublicationYear { get; set; }
        
        [Required]
        [MaxLength(Constants.BookTitleMaxLength)]
        public string AuthorName { get; set; } = string.Empty;
        public int ViewsCount { get; set; }
        public bool IsDeleted { get; set; }
    }
}