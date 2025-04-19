using DataAccess.Postgres.Migration;
using EquipmentInventory.API.Data;
using EquipmentInventory.API.Data.Models;
using EquipmentInventory.API.Helpers;
using EquipmentInventory.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult> AddTechnique([FromBody] TechniqueAddDto model)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        var validationResult = await _techniqueService.ValidateAddModelAsync(model);
        if (validationResult != null) return validationResult;

        try
        {
            var technique = await _techniqueService.CreateTechniqueAsync(model);
            return ApiResponseHelper.CreatedAt(nameof(GetTechnique), null, ApplicationErrors.SuccessfullyAdded);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize]
    [HttpPatch("update/{id}")]
    public async Task<ActionResult> UpdateTechnique(long id, [FromQuery] TechniqueUpdateDto model)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        var technique = await _dbContext.Techniques.FindAsync(id);
        if (technique == null)
            return ApiResponseHelper.NotFound(ApplicationErrors.NotFound);

        var validationResult = await _techniqueService.ValidateUpdateModelAsync(model, technique);
        if (validationResult != null) return validationResult;

        try
        {
            await _techniqueService.UpdateTechnique(technique, model);
            return ApiResponseHelper.Ok(ApplicationErrors.SuccessfullyUpdated);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    //[Authorize]
    [HttpGet("get")]
    public async Task<ActionResult<IEnumerable<TechniqueDto>>> GetTechnique(
        [FromQuery] PaginationModel pagination,
        [FromQuery] TechniqueFiltersDto filter)
    {
        try
        {
            var query = _techniqueService.ApplyFilter(filter, out int filterCount);
            if (filterCount > 1)
                return ApiResponseHelper.BadRequest(ApplicationErrors.FiltrationRestriction);

            var result = await _techniqueService.GetPaginatedResults(query, pagination);

            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
            return Ok(result.Items);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize]
    [HttpGet("outdated")]
    public async Task<ActionResult> GetOutdated(
        [FromQuery] PaginationModel pagination, 
        [FromQuery] BaseModel model)
    {
        if (int.TryParse(model.Content, out int yearsDifference))
        {
            var query = _techniqueService.GetOutdatedQuery(yearsDifference);
            var result = await _techniqueService.GetPaginatedResults(query, pagination);

            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
            return Ok(result.Items);
        }
        else
        {
            return ApiResponseHelper.BadRequest(ApplicationErrors.IncorrectFormat);
        }
    }

    [Authorize]
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteTechnique(long id)
    {
        var technique = await _dbContext.Techniques.FindAsync(id);
        if (technique == null)
            return ApiResponseHelper.NotFound(ApplicationErrors.NotFound);

        _dbContext.Techniques.Remove(technique);

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
