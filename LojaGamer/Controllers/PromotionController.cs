using LojaGamer.Data;
using LojaGamer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class PromotionController : ControllerBase
{
    private readonly AppDbContext _context;

    public PromotionController(AppDbContext context)
    {
        _context = context;
    }
    [HttpPost]
    public async Task<IActionResult> CreatePromotion(Promotion promotion)
    {
        // Remove Id para evitar conflito
        promotion.Id = 0; // ou ignore se já for padrão

        // Verifica se game existe
        var game = await _context.Games.FindAsync(promotion.GameId);
        if (game == null)
            return BadRequest($"Game with Id {promotion.GameId} not found.");

        promotion.Game = null;

        _context.Promotions.Add(promotion);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPromotion), new { id = promotion.Id }, promotion);
    }


    //[HttpPost]
    //public async Task<IActionResult> CreatePromotion(Promotion promotion)
    //{
    //    // Verifica se o GameId existe
    //    var game = await _context.Games.FindAsync(promotion.GameId);
    //    if (game == null)
    //        return BadRequest($"Game with Id {promotion.GameId} does not exist.");

    //    // Só mantemos o GameId, limpamos a referência para evitar conflito EF
    //    promotion.Game = null;

    //    _context.Promotions.Add(promotion);
    //    await _context.SaveChangesAsync();

    //    return CreatedAtAction(nameof(GetPromotion), new { id = promotion.Id }, promotion);
    //}

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePromotion(int id, Promotion promotion)
    {
        if (id != promotion.Id)
            return BadRequest("Id mismatch.");

        var promoInDb = await _context.Promotions
            .Include(p => p.Game)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (promoInDb == null)
            return NotFound();

        // Atualiza campos simples
        promoInDb.Name = promotion.Name;
        promoInDb.DiscountPercentage = promotion.DiscountPercentage;
        promoInDb.StartDate = promotion.StartDate;
        promoInDb.EndDate = promotion.EndDate;

        // Verifica se quer mudar o GameId (se permitido)
        if (promoInDb.GameId != promotion.GameId)
        {
            var game = await _context.Games.FindAsync(promotion.GameId);
            if (game == null)
                return BadRequest($"Game with Id {promotion.GameId} does not exist.");

            promoInDb.GameId = promotion.GameId;
        }

        await _context.SaveChangesAsync();

        return Ok(promoInDb);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPromotion(int id)
    {
        var promo = await _context.Promotions
            .Include(p => p.Game)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (promo == null) return NotFound();
        return Ok(promo);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetPromotions()
    {
        var promos = await _context.Promotions.Include(p => p.Game).ToListAsync();
        return Ok(promos);
    }
}
