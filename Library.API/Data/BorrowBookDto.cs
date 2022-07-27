using System.ComponentModel.DataAnnotations;

namespace Library.API.Data
{
    public class BorrowBookDto
    {
        [Required]
        public int UserID { get; set; }
        [Required]
        public int BookID { get; set; }

    }
}