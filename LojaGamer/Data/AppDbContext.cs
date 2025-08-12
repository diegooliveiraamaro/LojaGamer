using LojaGamer.Entities;
using Microsoft.EntityFrameworkCore;

namespace LojaGamer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Game> Games => Set<Game>();
        public DbSet<Promotion> Promotions => Set<Promotion>();

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // Email único para usuários
        //    modelBuilder.Entity<User>()
        //        .HasIndex(u => u.Email)
        //        .IsUnique();

        //    // Relacionamento Game -> Promotion (1:N)
        //    modelBuilder.Entity<Promotion>()
        //        .HasOne(p => p.Game)
        //        .WithMany(g => g.Promotions)
        //        .HasForeignKey(p => p.GameId)
        //        .OnDelete(DeleteBehavior.Cascade);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Email único para usuários
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Relacionamento User -> Games (1:N)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Games)
                .WithOne(g => g.User)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento Game -> Promotion (1:N)
            modelBuilder.Entity<Promotion>()
                .HasOne(p => p.Game)
                .WithMany(g => g.Promotions)
                .HasForeignKey(p => p.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
