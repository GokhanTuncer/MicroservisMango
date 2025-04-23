using Mango.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductID = 1,
                Name = "Cheeseburger",
                Price = 45,
                Description = "Klasik dana eti burger, cheddar peyniri ile servis edilir.",
                ImageUrl = "https://placehold.co/603x403",
                CategoryName = "Burger"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductID = 2,
                Name = "Pepperoni Pizza",
                Price = 60,
                Description = "İnce hamur üzerine bol mozzarella ve pepperoni dilimleri.",
                ImageUrl = "https://placehold.co/603x403",
                CategoryName = "Pizza"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductID = 3,
                Name = "Tavuklu Wrap",
                Price = 35,
                Description = "Izgara tavuk, yeşillik ve özel sos ile lavaşa sarılmış lezzet.",
                ImageUrl = "https://placehold.co/603x403",
                CategoryName = "Wrap"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductID = 4,
                Name = "Patates Kızartması",
                Price = 20,
                Description = "Dışı çıtır, içi yumuşak klasik patates kızartması.",
                ImageUrl = "https://placehold.co/603x403",
                CategoryName = "Appetizer"
            });
        }
    }
}
