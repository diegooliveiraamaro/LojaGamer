using LojaGamer.Enums;

namespace LojaGamer.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }

        public ICollection<Game> Games { get; set; } = new List<Game>();
    }

}
