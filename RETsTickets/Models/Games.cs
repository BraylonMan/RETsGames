using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RETsGames.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public string title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string Owner { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }

        //foeign key
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [NotMapped]
        [Display(Name = "Photo")]
        public IFormFile? FormFile { get; set; } //nullable
    }
}