using Microsoft.EntityFrameworkCore;
using Uweb4Media.Domain.Entities;
using Uweb4Media.Domain.Enums;

namespace Uweb4Media.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<MediaContent> MediaContents { get; set; } = default!; // ✅ YENİ

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User ayarları
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            // MediaContent ayarları
            modelBuilder.Entity<MediaContent>(entity =>
            {
                entity.Property(e => e.Sector).HasConversion<string>();
                entity.Property(e => e.Channel).HasConversion<string>();
                entity.Property(e => e.ContentType).HasConversion<string>();
            });
        }
    }
}