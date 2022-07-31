using System.ComponentModel.DataAnnotations;

namespace Library.API.Data;

public class UserDto
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }
}
