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

        var technique = await _techniqueService.CreateTechniqueAsync(model);
        return ApiResponseHelper.CreatedAt(nameof(GetTechnique), null, ApplicationErrors.TechniqueAdded);
    }

    [Authorize]
    [HttpPatch("update/{id}")]
    public async Task<IActionResult> UpdateTechnique(long id, [FromQuery] TechniqueUpdateDto model)
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
    public async Task<ActionResult<IEnumerable<TechniqueDto>>> GetTechnique(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5,
        [FromQuery] TechniqueUpdateDto filter = null)
    {
        try
        {
            var query = _techniqueService.ApplyFilter(filter, out int filterCount);

            if (filterCount > 1)
                return BadRequest(ApiResponse.BadRequest(ApplicationErrors.FiltrationRestriction));

            var result = await _techniqueService.GetPaginatedResults(query, page, pageSize);

            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
            return result.TotalCount > 0
                ? Ok(result.Items)
                : NotFound(ApiResponse.NotFound(ApplicationErrors.NotFound));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse.BadRequest(ex.Message));
        }
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
