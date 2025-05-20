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
        public DbSet<GameImage> GameImages { get; set; }
        public DbSet<UserGameGroup> UserGameGroups { get; set; }
        public DbSet<UserGameGroupGame> UserGameGroupGames { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameGenre>()
                .HasKey(gg => new { gg.GameId, gg.GenreId });

            modelBuilder.Entity<GamePlatform>()
                .HasKey(gp => new { gp.GameId, gp.PlatformId });

            modelBuilder.Entity<Wishlist>()
                .HasKey(w => new { w.UserId, w.GameId });

            modelBuilder.Entity<Library>()
                .HasKey(l => new { l.UserId, l.GameId });

            modelBuilder.Entity<GameImage>()
                .HasKey(gi => gi.Id);

            modelBuilder.Entity<GameImage>()
                .HasOne(gi => gi.Game)
                .WithMany(g => g.GameImages)
                .HasForeignKey(gi => gi.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.Game)
                .WithMany()
                .HasForeignKey(w => w.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Library>()
                .HasOne(l => l.Game)
                .WithMany()
                .HasForeignKey(l => l.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Game)
                .WithMany()
                .HasForeignKey(p => p.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserGameGroup>()
    .HasKey(ugg => ugg.Id);

            modelBuilder.Entity<UserGameGroup>()
                .HasOne(ugg => ugg.User)
                .WithMany(u => u.UserGameGroups)
                .HasForeignKey(ugg => ugg.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserGameGroupGame>()
                .HasKey(uggg => new { uggg.UserGameGroupId, uggg.GameId });

            modelBuilder.Entity<UserGameGroupGame>()
                .HasOne(uggg => uggg.UserGameGroup)
                .WithMany(ugg => ugg.UserGameGroupGames)
                .HasForeignKey(uggg => uggg.UserGameGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserGameGroupGame>()
                .HasOne(uggg => uggg.Game)
                .WithMany()
                .HasForeignKey(uggg => uggg.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=gamelauncher;Trusted_Connection=True;");
        }
    }
}
