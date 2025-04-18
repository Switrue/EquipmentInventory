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
public class OfficesController : ControllerBase
{
    private readonly EquipmentInventoryDbContext _dbContext;
    private readonly OfficesService _officesService;

    public OfficesController(
        EquipmentInventoryDbContext dbContext,
        OfficesService officesService)
    {
        _dbContext = dbContext;
        _officesService = officesService;
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPost("add")]
    public async Task<ActionResult> AddOffice([FromBody] OfficeAddDto model)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        var validationResult = await _officesService.ValidateAddModelAsync(model);
        if (validationResult != null) return validationResult;

        try
        {
            var office = await _officesService.CreateOfficeAsync(model);
            return ApiResponseHelper.CreatedAt(nameof(GetOffices), null, ApplicationErrors.SuccessfullyAdded);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPatch("update/{id}")]
    public async Task<ActionResult> UpdateOffice(long id, [FromQuery] OfficeUpdateDto model)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        var office = await _dbContext.Offices.FindAsync(id);
        if (office == null)
            return ApiResponseHelper.NotFound(ApplicationErrors.NotFound);

        var validationResult = await _officesService.ValidateUpdateModelAsync(model, office);
        if (validationResult != null) return validationResult;

        try
        {
            await _officesService.UpdateOfficeAsync(office, model);
            return ApiResponseHelper.Ok(ApplicationErrors.SuccessfullyUpdated);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet("get")]
    public async Task<ActionResult> GetOffices([FromQuery] PaginationModel pagination)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        try
        {
            var items = await _officesService.GetPaginatedResults(pagination);
            return Ok(items);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteOffice(long id)
    {
        var office = await _dbContext.Offices.FindAsync(id);
        if (office == null)
            return ApiResponseHelper.NotFound(ApplicationErrors.NotFound);

        _dbContext.Offices.Remove(office);

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
