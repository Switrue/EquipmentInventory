using DataAccess.Postgres.Migration.Models;
using DataAccess.Postgres.Migration;
using EquipmentInventory.API.Data.Models;
using EquipmentInventory.API.Helpers;
using Microsoft.EntityFrameworkCore;
using EquipmentInventory.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentInventory.API.Services;

public class TechniqueService
{
    private readonly EquipmentInventoryDbContext _context;
    private readonly EntityValidator _entityValidator;

    public TechniqueService(
        EquipmentInventoryDbContext context, 
        EntityValidator entityValidator)
    {
        _context = context;
        _entityValidator = entityValidator;
    }

    #region Get
    public IQueryable<Technique> ApplyFilter(TechniqueUpdateDto filter, out int filterCount)
    {
        var query = _context.Techniques.AsNoTracking();

        // Проверка количества фильтров
        var filters = new object?[] {
            filter?.Number,
            filter?.IdTypeTechnique,
            filter?.IdMember,
            filter?.Name,
            filter?.IdComputer,
            filter?.IdOffice,
            filter?.IdSupplier,
            filter?.UnderRepair,
            filter?.DateOfPurchase,
            filter?.DateOfManufacture,
            filter?.DateOfUse
        };

        filterCount = filters.Count(f => f != null);

        // Применение фильтра
        return filter switch
        {
            { Number: { } v } => query.Where(t => t.Number == v),
            { IdTypeTechnique: { } v } => query.Where(t => t.IdTypeTechnique == v),
            { Name: { } v } => query.Where(t => t.Name == v),
            { IdMember: { } v } => query.Where(t => t.IdMember == v),
            { IdComputer: { } v } => query.Where(t => t.IdComputer == v),
            { IdOffice: { } v } => query.Where(t => t.IdOffice == v),
            { IdSupplier: { } v } => query.Where(t => t.IdSupplier == v),
            { UnderRepair: { } v } => query.Where(t => t.UnderRepair == v),
            { DateOfPurchase: { } v } => query.Where(t => t.DateOfPurchase == v),
            { DateOfManufacture: { } v } => query.Where(t => t.DateOfManufacture == v),
            { DateOfUse: { } v } => query.Where(t => t.DateOfUse == v),
            { Cost: { } v } => query.Where(t => t.Cost == v),
            _ => query.OrderBy(t => t.Id)
        };
    }

    public async Task<PaginatedResult<TechniqueDto>> GetPaginatedResults(
        IQueryable<Technique> query,
        PaginationModel pagination)
    {
        if (pagination.Page.HasValue && pagination.PageSize.HasValue)
        {
            query = query
                .Skip((pagination.Page.Value - 1) * pagination.PageSize.Value)
                .Take(pagination.PageSize.Value);
        }

        var items = await query
            .Select(t => new TechniqueDto(
                t.Id,
                t.Number,
                t.IdTypeTechniqueNavigation.Name,
                t.Name,
                t.IdMemberNavigation != null
                    ? $"{t.IdMemberNavigation.Surname} {t.IdMemberNavigation.Username}"
                    : null,
                t.IdOfficeNavigation.Number,
                t.IdComputerNavigation.Number,
                t.DateOfPurchase,
                t.DateOfManufacture,
                t.DateOfUse,
                t.IdSupplierNavigation.Name,
                t.Cost,
                t.UnderRepair
            ))
            .ToListAsync();

        var totalCount = await query.CountAsync();

        return new PaginatedResult<TechniqueDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = pagination.Page ?? 1,
            PageSize = pagination.PageSize ?? items.Count()
        };
    }
    #endregion

    #region Add
    public async Task<Technique> CreateTechniqueAsync(TechniqueAddDto model)
    {
        var technique = new Technique
        {
            Number = model.Number,
            IdTypeTechnique = model.IdTypeTechnique,
            Name = model.Name,
            IdMember = model.IdMember,
            IdOffice = model.IdOffice,
            IdComputer = model.IdComputer,
            DateOfPurchase = model.DateOfPurchase,
            UnderRepair = model.UnderRepair ?? false,
            DateOfManufacture = model.DateOfManufacture,
            IdSupplier = model.IdSupplier,
            Cost = model.Cost,
            DateOfUse = model.DateOfUse
        };

        _context.Techniques.Add(technique);
        await _context.SaveChangesAsync();
        return technique;
    }
    #endregion

    #region Update
    public async Task UpdateTechnique(Technique existing, TechniqueUpdateDto model)
    {
        if (model.Number != null) existing.Number = model.Number;
        if (model.IdTypeTechnique.HasValue) existing.IdTypeTechnique = model.IdTypeTechnique.Value;
        if (model.Name != null) existing.Name = model.Name;
        if (model.IdMember != null) existing.IdMember = model.IdMember;
        if (model.IdOffice != null) existing.IdOffice = model.IdOffice;
        if (model.IdComputer != null) existing.IdComputer = model.IdComputer;
        if (model.DateOfPurchase.HasValue) existing.DateOfPurchase = model.DateOfPurchase.Value;
        if (model.DateOfManufacture.HasValue) existing.DateOfManufacture = model.DateOfManufacture.Value;
        if (model.DateOfUse != null) existing.DateOfUse = model.DateOfUse;
        if (model.IdSupplier.HasValue) existing.IdSupplier = model.IdSupplier.Value;
        if (model.Cost.HasValue) existing.Cost = model.Cost.Value;
        if (model.UnderRepair.HasValue) existing.UnderRepair = model.UnderRepair.Value;

        await _context.SaveChangesAsync();
    }
    #endregion

    #region Validation Helpers
    public async Task<ActionResult> ValidateAddModelAsync(TechniqueAddDto model)
    {
        if (!await _entityValidator.IsUniqueAsync(_context.Techniques, p => p.Number, model.Number))
            return ApiResponseHelper.BadRequest(ApplicationErrors.UniqueNumber);

        if (model.IdComputer.HasValue &&
            !await _entityValidator.IsUniqueAsync(_context.Techniques, p => p.IdComputer, model.IdComputer.Value))
            return ApiResponseHelper.BadRequest(ApplicationErrors.UniqueComputer);

        var validations = new List<(bool Result, string Error)>
        {
            (await _entityValidator.ExistsAsync<TypeTechnique>(model.IdTypeTechnique),
                ApplicationErrors.IncorrectTechniqueType),
            (await _entityValidator.ExistsAsync<Supplier>(model.IdSupplier),
                ApplicationErrors.IncorrectSupplier),
            (await _entityValidator.ExistsOptionalAsync<Member>(model.IdMember),
                ApplicationErrors.EmployeeNotFound),
            (await _entityValidator.ExistsOptionalAsync<Office>(model.IdOffice),
                ApplicationErrors.OfficeNotFound),
            (await _entityValidator.ExistsOptionalAsync<Computer>(model.IdComputer),
                ApplicationErrors.ComputerNotFound)
        };

        var error = validations.FirstOrDefault(v => !v.Result).Error;
        return error != null ? ApiResponseHelper.BadRequest(error) : null;
    }

    public async Task<ActionResult> ValidateUpdateModelAsync(TechniqueUpdateDto model, Technique existing)
    {
        if (!await _entityValidator.IsUniqueAsync(_context.Techniques, p => p.Number, model.Number, existing.Id))
            return ApiResponseHelper.BadRequest(ApplicationErrors.UniqueNumber);

        if (model.IdComputer.HasValue &&
            !await _entityValidator.IsUniqueAsync(_context.Techniques, p => p.IdComputer, model.IdComputer.Value, existing.Id))
            return ApiResponseHelper.BadRequest(ApplicationErrors.UniqueComputer);

        var validations = new List<(bool Result, string Error)>
        {
            (await _entityValidator.ValidateReferenceAsync<TypeTechnique>(model.IdTypeTechnique),
                ApplicationErrors.IncorrectTechniqueType),
            (await _entityValidator.ValidateReferenceAsync<Supplier>(model.IdSupplier),
                ApplicationErrors.IncorrectSupplier),
            (await _entityValidator.ExistsOptionalAsync<Member>(model.IdMember),
                ApplicationErrors.EmployeeNotFound),
            (await _entityValidator.ExistsOptionalAsync<Office>(model.IdOffice),
                ApplicationErrors.OfficeNotFound),
            (await _entityValidator.ExistsOptionalAsync<Computer>(model.IdComputer),
                ApplicationErrors.ComputerNotFound)
        };

        var error = validations.FirstOrDefault(v => !v.Result).Error;
        return error != null ? ApiResponseHelper.BadRequest(error) : null;
    }
    #endregion
}
