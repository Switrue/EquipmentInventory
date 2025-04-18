using DataAccess.Postgres.Migration;
using EquipmentInventory.API.Data.Models;
using EquipmentInventory.API.Data;
using EquipmentInventory.API.Helpers;
using EquipmentInventory.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentInventory.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TypeTechniqueController : ControllerBase
{
    private readonly EquipmentInventoryDbContext _dbContext;
    private readonly TypeTechniqueService _typeTechniqueService;

    public TypeTechniqueController(
        EquipmentInventoryDbContext dbContext,
        TypeTechniqueService typeTechniqueService)
    {
        _dbContext = dbContext;
        _typeTechniqueService = typeTechniqueService;
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPost("add")]
    public async Task<ActionResult> AddTypeTechnique([FromBody] BaseAddDto model)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        var validationResult = await _typeTechniqueService.ValidateAddModelAsync(model);
        if (validationResult != null) return validationResult;

        try
        {
            var typeTechnique = await _typeTechniqueService.CreateTypeTechniqueAsync(model);
            return ApiResponseHelper.CreatedAt(nameof(GetTypeTechnique), null, ApplicationErrors.SuccessfullyAdded);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPatch("update/{id}")]
    public async Task<ActionResult> UpdateTypeTechnique(
        long id,
        [FromQuery] BaseUpdateDto model)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        var typeTechnique = await _dbContext.TypeTechniques.FindAsync(id);
        if (typeTechnique == null)
            return ApiResponseHelper.NotFound(ApplicationErrors.NotFound);

        var validationResult = await _typeTechniqueService.ValidateUpdateModelAsync(model, typeTechnique);
        if (validationResult != null) return validationResult;

        try
        {
            await _typeTechniqueService.UpdateTypeTechnique(typeTechnique, model);
            return ApiResponseHelper.Ok(ApplicationErrors.SuccessfullyUpdated);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet("get")]
    public async Task<ActionResult> GetTypeTechnique([FromQuery] PaginationModel pagination)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        try
        {
            var items = await _typeTechniqueService.GetPaginatedResults(pagination);
            return Ok(items);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteTypeTechnique(long id)
    {
        var typeTechnique = await _dbContext.TypeTechniques.FindAsync(id);
        if (typeTechnique == null)
            return ApiResponseHelper.NotFound(ApplicationErrors.NotFound);

        _dbContext.TypeTechniques.Remove(typeTechnique);

        try
        {
            await _dbContext.SaveChangesAsync();
            return ApiResponseHelper.Ok(ApplicationErrors.DeleteSuccessfully);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }
}
