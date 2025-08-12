using LojaGamer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LojaGamer.Data
{
    //public class AppDbContext : DbContext
    //{
    //    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    //    public DbSet<User> Users => Set<User>();
    //    public DbSet<Game> Games => Set<Game>();
    //}
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Game> Games => Set<Game>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Definindo Email como único
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Outras configurações se necessário
        }
    }

}
