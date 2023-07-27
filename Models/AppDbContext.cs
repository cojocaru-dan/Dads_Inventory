using Microsoft.EntityFrameworkCore;

namespace DadsInventory.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Category>().HasData(new Category {CategoryId = 4, CategoryName = "Large"});
            modelBuilder.Entity<Category>().HasData(new Category {CategoryId = 5, CategoryName = "Medium"});
            modelBuilder.Entity<Category>().HasData(new Category {CategoryId = 6, CategoryName = "Small"});

            modelBuilder.Entity<Item>().HasData(
                new Item {Id = 1, Name = "Ship from fb", InStock = true, CategoryId = 4, Price = 199.95m},
                new Item {Id = 2, Name = "Hammer from db", InStock = false, CategoryId = 5, Price = 7.95m},
                new Item {Id = 3, Name = "Pin from db", InStock = true, CategoryId = 6, Price = 1.95m}
            );
        }
    }
}