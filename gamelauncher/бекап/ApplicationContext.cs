using Microsoft.EntityFrameworkCore; 

namespace gamelauncher.Model
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<GameGenre> GameGenres { get; set; }
        public DbSet<GamePlatform> GamePlatforms { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Library> Library { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<GameGroup> GameGroups { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameGenre>()
                .HasKey(gg => new { gg.GameId, gg.GenreId });

            modelBuilder.Entity<GameGroup>()
                .HasKey(gg => new { gg.GameId, gg.GroupId });

            modelBuilder.Entity<GamePlatform>()
                .HasKey(gp => new { gp.GameId, gp.PlatformId });

            modelBuilder.Entity<Wishlist>()
                .HasKey(w => new { w.UserId, w.GameId });

            modelBuilder.Entity<Library>()
                .HasKey(l => new { l.UserId, l.GameId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=gamelauncher;Trusted_Connection=True;");
        }
    }
}
