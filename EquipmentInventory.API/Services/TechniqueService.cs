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

    public async Task<bool> IsNumberUniqueAsync(string number, long? excludeId = null)
        => !await _context.Techniques
            .AnyAsync(t => t.Number == number && (excludeId == null || t.Id != excludeId));

    public async Task<bool> IsComputerAvailableAsync(long computerId, long? excludeId = null)
        => !await _context.Techniques
            .AnyAsync(t => t.IdComputer == computerId && (excludeId == null || t.Id != excludeId));

    public async Task<Technique> CreateTechniqueAsync(TechniqueAddModel model)
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

    public void UpdateTechnique(Technique existing, TechniqueUpdateModel model)
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
    }

    #region Validation Helpers
    public async Task<ActionResult> ValidateAddModelAsync(TechniqueAddModel model)
    {
        if (!await IsNumberUniqueAsync(model.Number))
            return ApiResponseHelper.BadRequest(ApplicationErrors.UniqueNumber);

        if (model.IdComputer.HasValue &&
            !await IsComputerAvailableAsync(model.IdComputer.Value))
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

    public async Task<ActionResult> ValidateUpdateModelAsync(TechniqueUpdateModel model, Technique existing)
    {
        if (model.Number != null && !await IsNumberUniqueAsync(model.Number, existing.Id))
            return ApiResponseHelper.BadRequest(ApplicationErrors.UniqueNumber);

        if (model.IdComputer.HasValue &&
            !await IsComputerAvailableAsync(model.IdComputer.Value, existing.Id))
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
