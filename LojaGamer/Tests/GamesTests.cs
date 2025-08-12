using FluentAssertions;
using LojaGamer.Data;
using LojaGamer.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LojaGamer.Tests
{
    public class GamesTests
    {
        private async Task<AppDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"gamesDb_{Guid.NewGuid()}")
                .Options;

            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureCreated();

            // Seed inicial
            dbContext.Games.Add(new Game { Title = "Halo", Genre = "FPS", Price = 199.90m });
            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        [Fact]
        public async Task GetGames_ShouldReturnGames()
        {
            var context = await GetDbContext();

            var games = await context.Games.ToListAsync();

            games.Should().NotBeNull();
            games.Count.Should().BeGreaterThan(0);
            games.First().Title.Should().Be("Halo");
        }

        [Fact]
        public async Task UpdateGame_ShouldChangeGameDetails()
        {
            var context = await GetDbContext();
            var game = await context.Games.FirstAsync();

            game.Title = "Halo Infinite";
            game.Price = 249.99m;

            context.Games.Update(game);
            await context.SaveChangesAsync();

            var updatedGame = await context.Games.FindAsync(game.Id);
            updatedGame.Title.Should().Be("Halo Infinite");
            updatedGame.Price.Should().Be(249.99m);
        }

        [Fact]
        public async Task DeleteGame_ShouldRemoveGameFromDatabase()
        {
            var context = await GetDbContext();
            var game = await context.Games.FirstAsync();

            context.Games.Remove(game);
            await context.SaveChangesAsync();

            var deletedGame = await context.Games.FindAsync(game.Id);
            deletedGame.Should().BeNull();
        }
    }
}
