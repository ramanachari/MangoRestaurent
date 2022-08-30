using Mango.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 1,
                Name = "Samosa",
                Price = 15,
                Description = "The samosas are a fried or baked pastry with a savory filling, such as spiced potatoes, onions, peas, lentils, and minced meat (lamb, beef or chicken).<br/>Sweet samosas are also sold in the cities of Pakistan including Peshawar; these sweet samosas contain no filling and are dipped in thick sugar syrup.",
                ImageUrl = "https://medhasridotnetmastery.blob.core.windows.net/mango/14.jpg",
                CategoryName = "Appetizer"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 2,
                Name = "Paneer Tikka",
                Price = 13.99,
                Description = "Paneer tikka is an Indian dish made from chunks of paneer marinated in spices and grilled in a tandoor.<br/>It is a vegetarian alternative to chicken tikka and other meat dishes. It is a popular dish that is widely available in India and countries with an Indian diaspora.",
                ImageUrl = "https://medhasridotnetmastery.blob.core.windows.net/mango/12.jpg",
                CategoryName = "Appetizer"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 3,
                Name = "Sweet Pie",
                Price = 10.99,
                Description = "A pie is a baked dish which is usually made of a pastry dough casing that contains a filling of various sweet or savoury ingredients.<br/>A filled pie (also single-crust or bottom-crust), has pastry lining the baking dish, and the filling is placed on top of the pastry but left open.",
                ImageUrl = "https://medhasridotnetmastery.blob.core.windows.net/mango/11.jpg",
                CategoryName = "Dessert"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 4,
                Name = "Pav Bhaji",
                Price = 15,
                Description = "Pav bhaji is a spiced mixture of mashed vegetables in a thick gravy served with bread.<br/>Vegetables in the curry may commonly include potatoes, onions, carrots, chillies, peas, bell peppers and tomatoes. Street sellers usually cook the curry on a flat griddle (tava) and serve the dish hot.",
                ImageUrl = "https://medhasridotnetmastery.blob.core.windows.net/mango/13.jpg",
                CategoryName = "Entree"
            });
        }

    }
}
