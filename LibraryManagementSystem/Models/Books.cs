// Book.cs

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        public string? Title { get; set; }

        public string? Author { get; set; }

        public string? ISBN { get; set; }

        public string? Category { get; set; }

        public int Stock { get; set; }

        public bool IsBorrowed { get; set; }
    }
}