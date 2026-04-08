using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace InventoryService.Infrastructure.Persistence
{
    public class InventoryDbContext : DbContext, IInventoryDbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<Category> Category => Set<Category>();        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            SeedData(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        //TODO: remove this, populate DB from migrations
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Science Fiction" },
                new Category { Id = 2, Name = "Fantasy" },
                new Category { Id = 3, Name = "History" }
            );

            // Seed Books
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Dune",
                    Author = "Frank Herbert",
                    ISBN = "978-0441013593",
                    Price = 9.99m,
                    StockQty = 10,
                    CategoryId = 1
                },
                new Book
                {
                    Id = 2,
                    Title = "The Hobbit",
                    Author = "J.R.R. Tolkien",
                    ISBN = "978-0547928227",
                    Price = 12.42m,
                    StockQty = 5,
                    CategoryId = 2
                }
            );
        }
    }
}
