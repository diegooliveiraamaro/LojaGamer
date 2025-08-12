using LojaGamer.Enums;

namespace LojaGamer.DTOs
{
    public class UserDto
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; } // já criptografada
        public UserRole Role { get; set; } // "User" ou "Admin"
    }
}
