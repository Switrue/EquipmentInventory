using DataAccess.Postgres.Migration.Models;
using DataAccess.Postgres.Migration;
using EquipmentInventory.API.Data.Models;
using EquipmentInventory.API.Data;
using EquipmentInventory.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EquipmentInventory.API.Services;

public class SuppliersService
{
    private readonly EquipmentInventoryDbContext _context;
    private readonly EntityValidator _entityValidator;

    public SuppliersService(
        EquipmentInventoryDbContext context,
        EntityValidator entityValidator)
    {
        _context = context;
        _entityValidator = entityValidator;
    }

    #region Get
    private IQueryable<Supplier> GetQuery()
    {
        return _context.Suppliers
            .AsNoTracking()
            .OrderBy(s => s.Id);
    }

    public async Task<IEnumerable<BaseDto>> GetPaginatedResults(PaginationModel pagination)
    {
        var query = GetQuery();

        if (pagination.Page.HasValue && pagination.PageSize.HasValue)
        {
            query = query
                .Skip((pagination.Page.Value - 1) * pagination.PageSize.Value)
                .Take(pagination.PageSize.Value);
        }

        var items = await query
            .Select(c => new BaseDto(
                c.Id,
                c.Name
            ))
            .ToListAsync();

        return items;
    }
    #endregion

    #region Add
    public async Task<Supplier> CreateSupplierAsync(BaseAddDto model)
    {
        var supplier = new Supplier
        {
            Name = model.Name
        };

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
        return supplier;
    }
    #endregion

    #region Update
    public async Task UpdateSupplier(Supplier existing, BaseUpdateDto model)
    {
        existing.Name = model.Name ?? existing.Name;

        await _context.SaveChangesAsync();
    }
    #endregion

    #region Validation Helpers
    public async Task<ActionResult> ValidateUpdateModelAsync(BaseUpdateDto model, Supplier existing)
    {
        if (!await _entityValidator.IsUniqueAsync(_context.Suppliers, p => p.Name, model.Name, existing.Id))
            return ApiResponseHelper.BadRequest(ApplicationErrors.UniqueName);

        return null;
    }

    public async Task<ActionResult> ValidateAddModelAsync(BaseAddDto model)
    {
        if (!await _entityValidator.IsUniqueAsync(_context.Suppliers, p => p.Name, model.Name))
            return ApiResponseHelper.BadRequest(ApplicationErrors.UniqueName);

        return null;
    }
    #endregion
}
