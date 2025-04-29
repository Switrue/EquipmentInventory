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
public class PositionsController : ControllerBase
{
    private readonly EquipmentInventoryDbContext _dbContext;
    private readonly PositionsServise _positionsServise;

    public PositionsController(
        EquipmentInventoryDbContext dbContext,
        PositionsServise positionsServise)
    {
        _dbContext = dbContext;
        _positionsServise = positionsServise;
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPost("add")]
    public async Task<ActionResult> AddPosition([FromBody] BaseAddDto model)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        var validationResult = await _positionsServise.ValidateAddModelAsync(model);
        if (validationResult != null) return validationResult;

        try
        {
            var position = await _positionsServise.CreatePositionAsync(model);
            return ApiResponseHelper.CreatedAt(nameof(GetPositions), null, ApplicationErrors.SuccessfullyAdded);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPatch("update/{id}")]
    public async Task<ActionResult> UpdatePosition(
        long id, 
        [FromQuery] BaseUpdateDto model)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        var position = await _dbContext.Positions.FindAsync(id);
        if (position == null)
            return ApiResponseHelper.NotFound(ApplicationErrors.NotFound);

        var validationResult = await _positionsServise.ValidateUpdateModelAsync(model, position);
        if (validationResult != null) return validationResult;

        try
        {
            await _positionsServise.UpdatePosition(position, model);
            return ApiResponseHelper.Ok(ApplicationErrors.SuccessfullyUpdated);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet("get")]
    public async Task<ActionResult> GetPositions([FromQuery] PaginationModel pagination)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        try
        {
            var items = await _positionsServise.GetPaginatedResults(pagination);
            return Ok(items);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeletePosition(long id)
    {
        var position = await _dbContext.Positions.FindAsync(id);
        if (position == null)
            return ApiResponseHelper.NotFound(ApplicationErrors.NotFound);

        if (await _positionsServise.HasDependenciesAsync(id))
            return ApiResponseHelper.BadRequest(ApplicationErrors.ViolatesTheRulesOfAddiction);

        _dbContext.Positions.Remove(position);

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
