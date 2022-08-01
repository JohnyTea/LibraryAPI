using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Library.API.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? UserName { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    [JsonIgnore]
    public List<Book> Books { get; set; } = new List<Book>();

}

