using LojaGamer.DTOs;
using LojaGamer.MongoModels;
using LojaGamer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LojaGamer.Controllers
{
    //[ApiController]
    //[Route("api/purchases")]
    //public class PurchaseHistoryController : ControllerBase
    //{
    //    private readonly MongoDbService _mongoService;

    //    public PurchaseHistoryController(MongoDbService mongoService)
    //    {
    //        _mongoService = mongoService;
    //    }

    //    [HttpPost]
    //    [Authorize]
    //    public async Task<IActionResult> AddPurchase(PurchaseDto dto)
    //    {
    //        var history = new GamePurchaseHistory
    //        {
    //            UserId = dto.UserId,
    //            GameTitle = dto.GameTitle
    //        };

    //        await _mongoService.AddAsync(history);
    //        return Ok(history);
    //    }

    //    [HttpGet]
    //    [Authorize(Roles = "Admin")]
    //    public async Task<IActionResult> GetAll() =>
    //        Ok(await _mongoService.GetAllAsync());
    //}

}
