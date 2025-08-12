namespace LojaGamer.Entities
{
    public class Promotion
    {
        public int Id { get; set; } // EF entende que é identity por padrão para int
        public string Name { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
    }

}
