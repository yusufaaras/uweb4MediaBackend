using Microsoft.EntityFrameworkCore;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uweb4Media.Domain.Entities;

namespace Uweb4Media.Persistence.Context
{
    public class WeddingHallContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=uweb4media_db;User Id=SA;Password=Yusuf123;Encrypt=False;TrustServerCertificate=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            modelBuilder.Entity<AppRole>().ToTable("AppRoles");
        } 
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppUser> AppUsers { get; set; } 
    }
}
