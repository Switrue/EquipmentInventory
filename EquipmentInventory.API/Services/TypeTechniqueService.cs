using DataAccess.Postgres.Migration.Models;
using EquipmentInventory.API.Data.Models;
using EquipmentInventory.API.Data;
using EquipmentInventory.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Postgres.Migration;

namespace EquipmentInventory.API.Services;

public class TypeTechniqueService
{
    private readonly EquipmentInventoryDbContext _context;
    private readonly EntityValidator _entityValidator;

    public TypeTechniqueService(
        EquipmentInventoryDbContext context,
        EntityValidator entityValidator)
    {
        _context = context;
        _entityValidator = entityValidator;
    }

    #region Get
    private IQueryable<TypeTechnique> GetQuery()
    {
        return _context.TypeTechniques
            .AsNoTracking()
            .OrderByDescending(t => t.Id);
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
    public async Task<TypeTechnique> CreateTypeTechniqueAsync(BaseAddDto model)
    {
        var typeTechnique = new TypeTechnique
        {
            Name = model.Name
        };

        _context.TypeTechniques.Add(typeTechnique);
        await _context.SaveChangesAsync();
        return typeTechnique;
    }
    #endregion

    #region Update
    public async Task UpdateTypeTechnique(TypeTechnique existing, BaseUpdateDto model)
    {
        existing.Name = model.Name ?? existing.Name;

        await _context.SaveChangesAsync();
    }
    #endregion

    #region Validation Helpers
    public async Task<ActionResult> ValidateUpdateModelAsync(BaseUpdateDto model, TypeTechnique existing)
    {
        if (!await _entityValidator.IsUniqueAsync(_context.TypeTechniques, p => p.Name, model.Name, existing.Id))
            return ApiResponseHelper.BadRequest(ApplicationErrors.UniqueName);
        return null;
    }

    public async Task<ActionResult> ValidateAddModelAsync(BaseAddDto model)
    {
        if (!await _entityValidator.IsUniqueAsync(_context.TypeTechniques, p => p.Name, model.Name))
            return ApiResponseHelper.BadRequest(ApplicationErrors.UniqueName);

        return null;
    }

    public async Task<bool> HasDependenciesAsync(long IdTypeTechnique)
        => await _context.Techniques.AnyAsync(t => t.IdTypeTechnique == IdTypeTechnique);
    #endregion
}
