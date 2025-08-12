namespace LojaGamer.DTOs
{
    public class GameDto
    {
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int UserId { get; set; }
    }
}
