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

            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.AppRole)
                .WithMany(r => r.AppUsers)
                .HasForeignKey(u => u.AppRoleID);

            // KULLANICI SİLİNDİĞİNDE MEDYA İÇERİKLERİ SİLİNSİN
            modelBuilder.Entity<MediaContent>()
                .HasOne(mc => mc.User)
                .WithMany(u => u.MediaContents)
                .HasForeignKey(mc => mc.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Kalsın. Kullanıcı silinince içerikleri silinsin.

            // KULLANICI SİLİNDİĞİNDE YORUMLARIN DAVRANIŞI
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction); // NO ACTION olarak kalmaya devam etsin.

            // İÇERİK SİLİNDİĞİNDE YORUMLARIN DAVRANIŞI
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.MediaContent)
                .WithMany(mc => mc.Comments)
                .HasForeignKey(c => c.MediaContentId)
                .OnDelete(DeleteBehavior.NoAction); // NO ACTION olarak kalmaya devam etsin.


            // KULLANICI SİLİNDİĞİNDE BEĞENİLERİN DAVRANIŞI - KRİTİK DÜZELTME BURADA!
            // Bu ilişkiyi NO ACTION olarak ayarlayarak çakışmayı önlüyoruz.
            // Beğeniler, MediaContent silindiğinde otomatik silinecekleri için dolaylı yoldan temizlenecektir.
            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.NoAction); // DEĞİŞİKLİK BURADA!

            // İÇERİK SİLİNDİĞİNDE BEĞENİLER SİLİNSİN
            modelBuilder.Entity<Like>()
                .HasOne(l => l.MediaContent)
                .WithMany(mc => mc.Likes)
                .HasForeignKey(l => l.MediaContentId)
                .OnDelete(DeleteBehavior.Cascade); // Bu CASCADE olarak kalmalı, MediaContent silinince beğenileri de silinsin.

            // ABONELİK İLİŞKİLERİ (Restrict olarak doğru ayarlanmışlar)
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Subscriber)
                .WithMany(u => u.SubscriptionsMade)
                .HasForeignKey(s => s.SubscriberUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Author)
                .WithMany(u => u.SubscriptionsReceived)
                .HasForeignKey(s => s.AuthorUserId)
                .OnDelete(DeleteBehavior.Restrict);


            // Benzersiz indeksler
            modelBuilder.Entity<Like>()
                .HasIndex(l => new { l.MediaContentId, l.UserId })
                .IsUnique();

            modelBuilder.Entity<Subscription>()
                .HasIndex(s => new { s.SubscriberUserId, s.AuthorUserId })
                .IsUnique();
        }

        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<MediaContent> MediaContents { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Firm> Firm { get; set; }
        public DbSet<Plans> Plans { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
    }
}