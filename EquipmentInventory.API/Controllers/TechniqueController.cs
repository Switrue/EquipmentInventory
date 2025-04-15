using DataAccess.Postgres.Migration;
using DataAccess.Postgres.Migration.Models;
using EquipmentInventory.API.Data;
using EquipmentInventory.API.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EquipmentInventory.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TechniqueController : ControllerBase
{
    private readonly EquipmentInventoryDbContext _dbContext;

    public TechniqueController(EquipmentInventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    //[Authorize]
    [HttpPost("add")]
    public async Task<ActionResult> AddTechnique([FromBody] TechniqueModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { Errors = ModelState });

        var validationResult = await ValidateTechniqueModelAsync(model);
        if (validationResult != null)
            return validationResult;

        var technique = CreateTechniqueEntity(model);

        try
        {
            await SaveTechniqueAsync(technique);
            return CreateSuccessResponse(technique.Id);
        }
        catch (DbUpdateException ex)
        {
            return HandleDatabaseError(ex);
        }
    }

    [Authorize]
    [HttpGet("get")]
    public async Task<ActionResult<IEnumerable<object>>> GetTechnique(int page, int pageSize)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _dbContext.Techniques
            .AsNoTracking()
            .OrderBy(t => t.Id);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
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
        Response.Headers.Append("X-Total-Count", totalCount.ToString());

        if (items.Count > 0)
            return Ok(items);
        else
            return NotFound(ApiResponse.NotFound(ApplicationErrors.NotFound));
    }

    //[Authorize]
    [HttpPatch("update/{id}")]
    public async Task<ActionResult> UpdateTechnique(long id, [FromBody] TechniqueModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { Errors = ModelState });

        var technique = await _dbContext.Techniques.FindAsync(id);
        if (technique == null)
            return NotFound(ApiResponse.NotFound(ApplicationErrors.NotFound));

        var validationResult = await ValidateUpdateModelAsync(model, technique);
        if (validationResult != null)
            return validationResult;

        UpdateTechniqueEntity(model, technique);

        try
        {
            await _dbContext.SaveChangesAsync();
            return Ok(ApiResponse.Ok(ApplicationErrors.TechniqueUpdated));
        }
        catch (DbUpdateException ex)
        {
            return HandleDatabaseError(ex);
        }
    }

    #region Helper Methods
    private async Task<ActionResult> ValidateTechniqueModelAsync(TechniqueModel model)
    {
        // Проверка уникальности
        if (await IsNumberExistsAsync(model.Number))
            return CreateErrorResponse(ApplicationErrors.UniqueNumber);

        if (await IsComputerAlreadyAssignedAsync(model.IdComputer))
            return CreateErrorResponse(ApplicationErrors.UniqueComputer);

        // Валидация связей
        var validationTasks = new List<Task<bool>>
        {
            ValidateEntityExistsAsync<TypeTechnique>(model.IdTypeTechnique),
            ValidateEntityExistsAsync<Supplier>(model.IdSupplier),
            ValidateOptionalEntityExistsAsync<Member>(model.IdMember),
            ValidateOptionalEntityExistsAsync<Office>(model.IdOffice),
            ValidateOptionalEntityExistsAsync<Computer>(model.IdComputer)
        };

        var validationResults = await Task.WhenAll(validationTasks);
        var errorMessages = new[]
        {
            ApplicationErrors.IncorrectTechniqueType,
            ApplicationErrors.IncorrectSupplier,
            ApplicationErrors.EmployeeNotFound,
            ApplicationErrors.OfficeNotFound,
            ApplicationErrors.ComputerNotFound
        };

        for (int i = 0; i < validationResults.Length; i++)
        {
            if (!validationResults[i])
                return CreateErrorResponse(errorMessages[i]);
        }

        return null;
    }

    private async Task<bool> IsNumberExistsAsync(string number)
        => await _dbContext.Techniques.AnyAsync(t => t.Number == number);

    private async Task<bool> IsComputerAlreadyAssignedAsync(long? computerId)
        => computerId.HasValue &&
           await _dbContext.Techniques.AnyAsync(t => t.IdComputer == computerId);

    private async Task<bool> ValidateEntityExistsAsync<T>(long id) where T : class
        => await _dbContext.Set<T>().AnyAsync(e => EF.Property<long>(e, "Id") == id);

    private async Task<bool> ValidateOptionalEntityExistsAsync<T>(long? id) where T : class
        => !id.HasValue || await ValidateEntityExistsAsync<T>(id.Value);

    private Technique CreateTechniqueEntity(TechniqueModel model) => new Technique
    {
        Number = model.Number,
        IdTypeTechnique = model.IdTypeTechnique,
        Name = model.Name,
        IdMember = model.IdMember,
        IdOffice = model.IdOffice,
        IdComputer = model.IdComputer,
        DateOfPurchase = model.DateOfPurchase,
        UnderRepair = model.UnderRepair,
        DateOfManufacture = model.DateOfManufacture,
        IdSupplier = model.IdSupplier,
        Cost = model.Cost,
        DateOfUse = model.DateOfUse
    };

    private async Task SaveTechniqueAsync(Technique technique)
    {
        _dbContext.Techniques.Add(technique);
        await _dbContext.SaveChangesAsync();
    }

    private ActionResult CreateSuccessResponse(long techniqueId)
        => CreatedAtAction(
            nameof(GetTechnique),
            ApiResponse.Ok(ApplicationErrors.TechniqueAdded)
        );

    private BadRequestObjectResult CreateErrorResponse(string message)
        => BadRequest(ApiResponse.BadRequest(message));

    private ActionResult HandleDatabaseError(DbUpdateException ex)
        => StatusCode(
            StatusCodes.Status500InternalServerError,
            ApiResponse.BadRequest(ApplicationErrors.UpdateError)
        );
    #endregion

    #region Update Helpers
    private async Task<ActionResult> ValidateUpdateModelAsync(TechniqueModel model, Technique existing)
    {
        if (model.Number != existing.Number && await IsNumberExistsAsync(model.Number))
            return CreateErrorResponse(ApplicationErrors.UniqueNumber);

        if (model.IdComputer != existing.IdComputer && await IsComputerAlreadyAssignedAsync(model.IdComputer))
            return CreateErrorResponse(ApplicationErrors.UniqueComputer);

        var validationTasks = new List<Task<bool>>
        {
            ValidateEntityExistsAsync<TypeTechnique>(model.IdTypeTechnique),
            ValidateEntityExistsAsync<Supplier>(model.IdSupplier),
            ValidateOptionalEntityExistsAsync<Member>(model.IdMember),
            ValidateOptionalEntityExistsAsync<Office>(model.IdOffice),
            ValidateOptionalEntityExistsAsync<Computer>(model.IdComputer)
        };

        var validationResults = await Task.WhenAll(validationTasks);

        var errorMessages = new[]
        {
            ApplicationErrors.IncorrectTechniqueType,
            ApplicationErrors.IncorrectSupplier,
            ApplicationErrors.EmployeeNotFound,
            ApplicationErrors.OfficeNotFound,
            ApplicationErrors.ComputerNotFound
        };

        for (int i = 0; i < validationResults.Length; i++)
        {
            if (!validationResults[i])
                return CreateErrorResponse(errorMessages[i]);
        }

        return null;
    }

    private void UpdateTechniqueEntity(TechniqueModel model, Technique entity)
    {
        entity.Number = model.Number;
        entity.IdTypeTechnique = model.IdTypeTechnique;
        entity.Name = model.Name;
        entity.IdMember = model.IdMember;
        entity.IdOffice = model.IdOffice;
        entity.IdComputer = model.IdComputer;
        entity.DateOfPurchase = model.DateOfPurchase;
        entity.UnderRepair = model.UnderRepair;
        entity.DateOfManufacture = model.DateOfManufacture;
        entity.IdSupplier = model.IdSupplier;
        entity.Cost = model.Cost;
        entity.DateOfUse = model.DateOfUse;
    }
    #endregion
}
