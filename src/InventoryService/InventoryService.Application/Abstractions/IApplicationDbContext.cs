using InventoryService.Domain.InventoryItems;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<InventoryItem> InventoryItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
