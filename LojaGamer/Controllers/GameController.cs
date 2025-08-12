using LojaGamer.Data;
using LojaGamer.DTOs;
using LojaGamer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LojaGamer.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GameController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GameController(AppDbContext context) => _context = context;

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateGame(GameDto dto)
        {
            var game = new Game
            {
                Title = dto.Title,
                Genre = dto.Genre,
                Price = dto.Price,
                UserId = dto.UserId
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return Ok(game);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetGames() => Ok(await _context.Games.ToListAsync());

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateGame(int id, GameDto dto)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
                return NotFound(new { message = "Jogo não encontrado" });

            game.Title = dto.Title;
            game.Genre = dto.Genre;
            game.Price = dto.Price;
            game.UserId = dto.UserId;

            await _context.SaveChangesAsync();
            return Ok(game);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
                return NotFound(new { message = "Jogo não encontrado" });

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }

}
