using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Library.API.Models;

public class Book
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [DefaultValue("Unknown")]
    public string? Author { get; set; }

    [DefaultValue("None")]
    public string? Publisher { get; set; }
    
    public int Year { get; set; }

    [Required]
    [MinLength(13)]
    [MaxLength(13)]
    public string ISBN { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Book book &&
               Title == book.Title &&
               Author == book.Author &&
               Publisher == book.Publisher &&
               Year == book.Year &&
               ISBN == book.ISBN;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Title, Author, Publisher, Year, ISBN);
    }
}

