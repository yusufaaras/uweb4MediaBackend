using Microsoft.EntityFrameworkCore;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uweb4Media.Domain.Entities;
using Like = Uweb4Media.Domain.Entities.Like;

namespace Uweb4Media.Persistence.Context
{
    public class Uweb4MediaContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=uweb4media_db;User Id=SA;Password=Yusuf123;Encrypt=False;TrustServerCertificate=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            modelBuilder.Entity<AppRole>().ToTable("AppRoles");
            
            // AppUser - MediaContent (1-N)
            modelBuilder.Entity<MediaContent>()
                .HasOne(mc => mc.User)
                .WithMany()
                .HasForeignKey(mc => mc.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Bir AppUser silinince MediaContent'ları silinsin

            // AppUser - Like (1-N)
            // Bu ilişkiyi NoAction olarak değiştiriyoruz.
            // Çünkü MediaContent üzerinden de bir cascade yolu var.
            modelBuilder.Entity<Like>()
                .HasOne(l => l.User) // Like sınıfında User prop'u olduğu için l.User kullanın.
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.NoAction); // ÖNEMLİ DEĞİŞİKLİK BURADA!

            // MediaContent - Like (1-N)
            modelBuilder.Entity<Like>()
                .HasOne(l => l.MediaContent)
                .WithMany(mc => mc.Likes)
                .HasForeignKey(l => l.MediaContentId)
                .OnDelete(DeleteBehavior.Cascade); // Bir MediaContent silinince Like'ları silinsin

            // AppUser - Comment (1-N)
            // Bu ilişkiyi NoAction olarak bırakıyoruz.
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction); 

            // MediaContent - Comment (1-N)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.MediaContent)
                .WithMany(mc => mc.Comments)
                .HasForeignKey(c => c.MediaContentId)
                .OnDelete(DeleteBehavior.Cascade); // Bir MediaContent silinince Comment'leri silinsin
        } 
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppUser> AppUsers { get; set; } 
        public DbSet<Comment> Comments { get; set; } 
        public DbSet<MediaContent> MediaContents { get; set; }
        public DbSet<Like> Likes { get; set; } 
    }
}