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
public class SuppliersController : ControllerBase
{
    private readonly EquipmentInventoryDbContext _dbContext;
    private readonly SuppliersService _suppliersService;

    public SuppliersController(
        EquipmentInventoryDbContext dbContext,
        SuppliersService suppliersService)
    {
        _dbContext = dbContext;
        _suppliersService = suppliersService;
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPost("add")]
    public async Task<ActionResult> AddSupplier([FromBody] BaseAddDto model)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        var validationResult = await _suppliersService.ValidateAddModelAsync(model);
        if (validationResult != null) return validationResult;

        try
        {
            var supplier = await _suppliersService.CreateSupplierAsync(model);
            return ApiResponseHelper.CreatedAt(nameof(GetSuppliers), null, ApplicationErrors.SuccessfullyAdded);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPatch("update/{id}")]
    public async Task<ActionResult> UpdateSupplier(
        long id,
        [FromQuery] BaseUpdateDto model)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        var supplier = await _dbContext.Suppliers.FindAsync(id);
        if (supplier == null)
            return ApiResponseHelper.NotFound(ApplicationErrors.NotFound);

        var validationResult = await _suppliersService.ValidateUpdateModelAsync(model, supplier);
        if (validationResult != null) return validationResult;

        try
        {
            await _suppliersService.UpdateSupplier(supplier, model);
            return ApiResponseHelper.Ok(ApplicationErrors.SuccessfullyUpdated);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet("get")]
    public async Task<ActionResult> GetSuppliers([FromQuery] PaginationModel pagination)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        try
        {
            var items = await _suppliersService.GetPaginatedResults(pagination);
            return Ok(items);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteSupplier(long id)
    {
        var supplier = await _dbContext.Suppliers.FindAsync(id);
        if (supplier == null)
            return ApiResponseHelper.NotFound(ApplicationErrors.NotFound);

        if (await _suppliersService.HasDependenciesAsync(id))
            return ApiResponseHelper.BadRequest(ApplicationErrors.ViolatesTheRulesOfAddiction);

        _dbContext.Suppliers.Remove(supplier);

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
