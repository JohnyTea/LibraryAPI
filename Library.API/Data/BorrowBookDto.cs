using System.ComponentModel.DataAnnotations;

namespace Library.API.Data;

public class BorrowedBookDto
{
    [Required]
    public int UserID { get; set; }

    [Required]
    public int BookID { get; set; }
}
