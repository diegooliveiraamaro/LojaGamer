using LojaGamer.Data;
using LojaGamer.Entities;
using Microsoft.EntityFrameworkCore;

namespace LojaGamer.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        //public async Task AddAsync(User user)
        //{
        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();
        //}

        public async Task<User?> Authenticate(string email, string password)
        {
            var user = await GetByEmailAsync(email);

            if (user == null)
                return null;

            bool passwordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            return passwordValid ? user : null;
        }
        //public async Task AddAsync(User user)
        //{
        //    if (await _context.Users.AnyAsync(u => u.Email == user.Email))
        //    {
        //        throw new InvalidOperationException("Email já cadastrado.");
        //    }

        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();
        //}
        public async Task AddAsync(User user)
        {
            bool emailExists = await _context.Users
                .AnyAsync(u => u.Email == user.Email);

            if (emailExists)
            {
                throw new InvalidOperationException("Email já cadastrado.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
