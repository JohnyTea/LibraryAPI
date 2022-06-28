using System.ComponentModel.DataAnnotations;


namespace Library.API.Models;

public class Book
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string? Author { get; set; }

    public string? Publisher { get; set; }

    public int Year { get; set; }

    [Required]
    public string ISBN { get; set; }

}

