using System.ComponentModel.DataAnnotations;

namespace Library.API.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public DateOnly BirthDate { get; set; }

}

