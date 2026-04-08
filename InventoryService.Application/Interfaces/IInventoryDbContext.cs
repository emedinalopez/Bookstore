using InventoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Application.Interfaces
{
    public interface IInventoryDbContext
    {
        DbSet<Book> Books { get; }
        DbSet<Category> Category { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
