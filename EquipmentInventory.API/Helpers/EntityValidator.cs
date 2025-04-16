using DataAccess.Postgres.Migration;
using Microsoft.EntityFrameworkCore;

namespace EquipmentInventory.API.Helpers;

public class EntityValidator
{
    private readonly EquipmentInventoryDbContext _context;

    public EntityValidator(EquipmentInventoryDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync<TEntity>(long id) where TEntity : class
        => await _context.Set<TEntity>().AnyAsync(e => EF.Property<long>(e, "Id") == id);

    public async Task<bool> ExistsOptionalAsync<TEntity>(long? id) where TEntity : class
        => !id.HasValue || await ExistsAsync<TEntity>(id.Value);

    public async Task<bool> ValidateReferenceAsync<TEntity>(long? id) where TEntity : class
        => !id.HasValue || await ExistsAsync<TEntity>(id.Value);
}
