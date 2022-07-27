using System.ComponentModel.DataAnnotations;

namespace Library.API.Data
{
    public struct BookDto
    {
        public string Author { get; set; }

        [Required]
        public string Title { get; set; }

        public string Publisher { get; set; }

        public int Year { get; set; }

        [Required]
        [MinLength(13)]
        [MaxLength(13)]
        public string ISBN { get; set; }
    }
}