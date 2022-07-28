using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

    public int? BorrowerID { get; set; }

    [JsonIgnore]
    public User? Borrower { get; private set; }


    public void SetBorrower(User borrower)
    {
        if (BorrowerID is not null) throw new InvalidOperationException("Book is already borrowed by another user. Send Get for more information."); 

        BorrowerID = borrower.Id;
        Borrower = borrower;
        borrower.Books.Add(this);
    }

    public void RemoveBorrower()
    {
        if (BorrowerID is null || Borrower is null) return;

        Borrower.Books.Remove(this);
        BorrowerID = null;
        Borrower = null;
    }

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

