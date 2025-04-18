using DataAccess.Postgres.Migration;
using DataAccess.Postgres.Migration.Models;
using EquipmentInventory.API.Data;
using EquipmentInventory.API.Data.Models;
using EquipmentInventory.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EquipmentInventory.API.Services;

public class OfficesService
{
    private readonly EquipmentInventoryDbContext _context;
    private readonly EntityValidator _entityValidator;

    public OfficesService(
        EquipmentInventoryDbContext context,
        EntityValidator entityValidator)
    {
        _context = context;
        _entityValidator = entityValidator;
    }

    #region Get
    private IQueryable<Office> GetQuery()
    {
        return _context.Offices
            .AsNoTracking()
            .OrderBy(o => o.Id);
    }

    public async Task<IEnumerable<OfficeDto>> GetPaginatedResults(PaginationModel pagination)
    {
        var query = GetQuery();

        if (pagination.Page.HasValue && pagination.PageSize.HasValue)
        {
            query = query
                .Skip((pagination.Page.Value - 1) * pagination.PageSize.Value)
                .Take(pagination.PageSize.Value);
        }

        var items = await query
            .Select(c => new OfficeDto(
                c.Id,
                c.Floor,
                c.Number,
                c.Name
            ))
            .ToListAsync();

        return items;
    }
    #endregion

    #region Add
    public async Task<Office> CreateOfficeAsync(OfficeAddDto model)
    {
        var office = new Office
        {
            Floor = model.Floor,
            Number = model.Number,
            Name = model.Name
        };

        _context.Offices.Add(office);
        await _context.SaveChangesAsync();
        return office;
    }
    #endregion

    #region Update
    public async Task UpdateOfficeAsync(Office existing, OfficeUpdateDto model)
    {
        existing.Floor = model.Floor ?? existing.Floor;
        existing.Number = model.Number ?? existing.Number;
        existing.Name = model.Name ?? existing.Name;

        await _context.SaveChangesAsync();
    }
    #endregion

    #region Validation Helpers
    public async Task<ActionResult> ValidateUpdateModelAsync(OfficeUpdateDto model, Office existing)
    {
        if (model.Number.HasValue && 
            !await _entityValidator.IsUniqueAsync(_context.Offices, p => p.Number, model.Number.Value, existing.Id))
            return ApiResponseHelper.BadRequest(ApplicationErrors.UniqueNumber);

        if (!await _entityValidator.IsUniqueAsync(_context.Offices, p => p.Name, model.Name, existing.Id))
            return ApiResponseHelper.BadRequest(ApplicationErrors.UniqueName);

        return null;
    }

    public async Task<ActionResult> ValidateAddModelAsync(OfficeAddDto model)
    {
        if (!await _entityValidator.IsUniqueAsync(_context.Offices, p => p.Number, model.Number))
            return ApiResponseHelper.BadRequest(ApplicationErrors.UniqueNumber);

        if (!await _entityValidator.IsUniqueAsync(_context.Offices, p => p.Name, model.Name))
            return ApiResponseHelper.BadRequest(ApplicationErrors.UniqueName);

        return null;
    }
    #endregion
}
