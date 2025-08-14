using Microsoft.EntityFrameworkCore;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uweb4Media.Domain.Entities;
using Uweb4Media.Domain.Entities.Admin.Campaign;
using Uweb4Media.Domain.Entities.Admin.Channel;
using Uweb4Media.Domain.Entities.Admin.CompanyManagement;
using Uweb4Media.Domain.Entities.Admin.Sector;
using Uweb4Media.Domain.Entities.Admin.Video;
using Uweb4Media.Domain.Entities.StripePayment;
using Like = Uweb4Media.Domain.Entities.Like;

namespace Uweb4Media.Persistence.Context
{
    public class Uweb4MediaContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=localhost,1433;Database=uweb4media_db;User Id=SA;Password=Yagmur2736;Encrypt=False;TrustServerCertificate=True");
            //optionsBuilder.UseSqlServer("Server=localhost,1433;Database=uweb4media_db;User Id=SA;Password=Yusuf123;Encrypt=False;TrustServerCertificate=True");

            optionsBuilder.UseSqlServer("Server=tcp:uweb4mediadb.database.windows.net,1433;Initial Catalog=uweb4media_db;Persist Security Info=False;User ID=web4Prime;Password=Uweb4.2025;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
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
                .HasOne(c => c.Video)
                .WithMany(mc => mc.Comments)
                .HasForeignKey(c => c.VideoId)
                .OnDelete(DeleteBehavior.NoAction); // NO ACTION olarak kalmaya devam etsin.
            
            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.NoAction); 
            
            modelBuilder.Entity<Like>()
                .HasOne(l => l.Video)
                .WithMany(mc => mc.Likes)
                .HasForeignKey(l => l.VideoId)
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

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.AuthorCompany)
                .WithMany() 
                .HasForeignKey(s => s.AuthorCompanyId)
                .OnDelete(DeleteBehavior.Restrict);


            // Benzersiz indeksler
            modelBuilder.Entity<Like>()
                .HasIndex(l => new { l.VideoId, l.UserId })
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
        public DbSet<Plans> Plans { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Sector> Sectors { get; set; } 
        public DbSet<Channel> Channels { get; set; } 
        public DbSet<Campaign> Campaigns { get; set; } 
        public DbSet<CampaignPerformance> CampaignPerformances { get; set; }  
        public DbSet<Company> Companies { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PartnerShare> PartnerShares { get; set; }
    }
}