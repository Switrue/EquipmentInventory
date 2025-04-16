using DataAccess.Postgres.Migration;
using EquipmentInventory.API.Data;
using EquipmentInventory.API.Data.Models;
using EquipmentInventory.API.Helpers;
using EquipmentInventory.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EquipmentInventory.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TechniqueController : ControllerBase
{
    private readonly EquipmentInventoryDbContext _dbContext;
    private readonly TechniqueService _techniqueService;

    public TechniqueController(
        EquipmentInventoryDbContext dbContext,
        TechniqueService techniqueService)
    {
        _dbContext = dbContext;
        _techniqueService = techniqueService;
    }

    [Authorize]
    [HttpPost("add")]
    public async Task<ActionResult> AddTechnique([FromBody] TechniqueAddModel model)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        var validationResult = await _techniqueService.ValidateAddModelAsync(model);
        if (validationResult != null) return validationResult;

        var technique = await _techniqueService.CreateTechniqueAsync(model);
        return ApiResponseHelper.CreatedAt(nameof(GetTechnique), null, ApplicationErrors.TechniqueAdded);
    }

    [Authorize]
    [HttpPatch("update/{id}")]
    public async Task<IActionResult> UpdateTechnique(long id, [FromBody] TechniqueUpdateModel model)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        var technique = await _dbContext.Techniques.FindAsync(id);
        if (technique == null)
            return ApiResponseHelper.NotFound(ApplicationErrors.NotFound);

        var validationResult = await _techniqueService.ValidateUpdateModelAsync(model, technique);
        if (validationResult != null) return validationResult;

        _techniqueService.UpdateTechnique(technique, model);

        try
        {
            await _dbContext.SaveChangesAsync();
            return Ok(ApiResponse.Ok(ApplicationErrors.TechniqueUpdated));
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.UpdateError);
        }
    }

    [Authorize]
    [HttpGet("get")]
    public async Task<ActionResult<IEnumerable<TechniqueDto>>> GetTechnique(int page = 1, int pageSize = 5)
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

    [Authorize]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteTechnique(long id)
    {
        var technique = await _dbContext.Techniques.FindAsync(id);
        if (technique == null)
            return NotFound(ApiResponse.NotFound(ApplicationErrors.NotFound));

        _dbContext.Techniques.Remove(technique);
        await _dbContext.SaveChangesAsync();

        return Ok(ApiResponse.Ok(ApplicationErrors.DeleteSuccessfully));
    }
}
