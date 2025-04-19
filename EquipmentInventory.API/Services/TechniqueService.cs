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
    public IQueryable<Technique> GetOutdatedQuery(int value)
    {
        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
        DateOnly thresholdDate = currentDate.AddYears(-value);

        var query = _context.Techniques
            .AsNoTracking()
            .Where(t => t.DateOfPurchase <= thresholdDate)
            .OrderBy(t => t.Id);

        return query;
    }

    public IQueryable<Technique> ApplyFilter(TechniqueFiltersDto filter, out int filterCount)
    {
        var query = _context.Techniques.AsNoTracking();

        // Проверка количества фильтров
        var filters = new object?[] {
            filter?.Number,
            filter?.TypeTechnique,
            filter?.Member,
            filter?.Name,
            filter?.Computer,
            filter?.Office,
            filter?.Supplier,
            filter?.UnderRepair,
            filter?.DateOfPurchase,
            filter?.DateOfManufacture,
            filter?.DateOfUse,
            filter?.Cost
        };

        filterCount = filters.Count(f => f != null);

        if (filter?.IsUnderRepair is { } isUnderRepair)
        {
            query = query.Where(t => t.UnderRepair == isUnderRepair);
        }

        if (filter?.IsFastened is { } isFastened) 
        {
            query = isFastened
                ? query.Where(t => t.IdMember != null || t.IdOffice != null)
                : query.Where(t => t.IdMember == null || t.IdOffice == null);
        }

        // Основной фильтр
        return ApplyFilters(query, filter);
    }

    private IQueryable<Technique> ApplyFilters(
        IQueryable<Technique> query,
        TechniqueFiltersDto? filter)
    {
        if (filter != null) 
        { 
            if (filter.Number is { } number)
                query = query.Where(t => t.Number.ToLower().Contains(number.ToLower()));

            if (filter.TypeTechnique is  { } typeTechnique)
                query = query.Where(t => t.IdTypeTechniqueNavigation.Name.ToLower().Contains(typeTechnique.ToLower()));

            if (filter.Name is { } name)
                query = query.Where(t => t.Name.ToLower().Contains(name.ToLower()));

            if (filter.Member is { } member)
                query = query.Where(t =>
                    t.IdMemberNavigation.Username.ToLower().Contains(member.ToLower()) ||
                    t.IdMemberNavigation.Surname.ToLower().Contains(member.ToLower()));

            if (filter.Computer is { } computer)
                query = query.Where(t => t.IdComputerNavigation.Number == computer);

            if (filter.Office is { } office)
                query = query.Where(t => t.IdOfficeNavigation.Number == office);

            if (filter.Supplier is { } supplier)
                query = query.Where(t => t.IdSupplierNavigation.Name.ToLower().Contains(supplier.ToLower()));

            if (filter.UnderRepair is { } underRepair)
                query = query.Where(t => t.UnderRepair == underRepair);

            if (filter.DateOfPurchase is { } dateOfPurchase)
                query = query.Where(t => t.DateOfPurchase == dateOfPurchase);

            if (filter.DateOfManufacture is { } dateOfManufacture)
                query = query.Where(t => t.DateOfManufacture == dateOfManufacture);

            if (filter.DateOfUse is { } dateOfUse)
                query = query.Where(t => t.DateOfUse == dateOfUse);

            if (filter.Cost is { } cost)
                query = query.Where(t => t.Cost == cost);
        }
        return query.OrderBy(t => t.Id);
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
