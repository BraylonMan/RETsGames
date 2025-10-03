namespace RETsGames.Models
{
    public class Category

    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public List<Game>? Game { get; set; }

    }
}
