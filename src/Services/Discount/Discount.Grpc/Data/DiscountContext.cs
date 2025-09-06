using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public class DiscountContext: DbContext
    {
        public DbSet<Coupan> Coupans { get; set; } = default;
        public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupan>().HasData(
                new Coupan
                {
                    Id = 1,
                    ProductName = "IPhone X",
                    Description = "IPhone Discount",
                    Amount = 150
                },
                new Coupan
                {
                    Id = 2,
                    ProductName = "Samsung 10",
                    Description = "Samsung Discount",
                    Amount = 100
                }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                return;
            }

            // Disable migration locking for SQLite in containers
            optionsBuilder.UseSqlite(options =>
            {
                options.CommandTimeout(30);
            });
        }
    }
}
