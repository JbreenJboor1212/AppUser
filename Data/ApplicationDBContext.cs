using AppUser.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppUser.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUserT>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions):base(dbContextOptions)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Add Has Data
            List<IdentityRole> roles = new List<IdentityRole>
            {
               new IdentityRole
               {
                Name = "Admin",
                NormalizedName ="ADMIN"
               },
               new IdentityRole
               {
                Name = "User",
                NormalizedName ="USER"
               },
            };

            builder.Entity<IdentityRole>().HasData(roles);

            //Many To Many [between Stock , AppUser , To Create Portfolio]
            builder.Entity<Portfolio>(
                x => x.HasKey(k => new { k.StockId, k.AppUserId })
                );

            builder.Entity<Portfolio>()
                .HasOne(u => u.Stock)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(u => u.StockId);

            builder.Entity<Portfolio>()
               .HasOne(u => u.AppUserT)
               .WithMany(u => u.Portfolios)
               .HasForeignKey(u => u.AppUserId);
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

    }
}
