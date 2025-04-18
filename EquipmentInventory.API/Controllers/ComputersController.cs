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
public class ComputersController : ControllerBase
{
    private readonly EquipmentInventoryDbContext _dbContext;
    private readonly ComputersService _computersService;

    public ComputersController(
        EquipmentInventoryDbContext dbContext,
        ComputersService computersService)
    {
        _dbContext = dbContext;
        _computersService = computersService;
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPost("add")]
    public async Task<ActionResult> AddComputer([FromBody] ComputerAddDto model)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        var validationResult = await _computersService.ValidateAddModelAsync(model);
        if (validationResult != null) return validationResult;

        try
        {
            var computer = await _computersService.CreateComputerAsync(model);
            return ApiResponseHelper.CreatedAt(nameof(GetComputers), null, ApplicationErrors.SuccessfullyAdded);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPatch("update/{id}")]
    public async Task<ActionResult> UpdateComputer(
        long id,
        [FromQuery] ComputerUpdateDto model)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        var computer = await _dbContext.Computers.FindAsync(id);
        if (computer == null)
            return ApiResponseHelper.NotFound(ApplicationErrors.NotFound);

        var validationResult = await _computersService.ValidateUpdateModelAsync(model, computer);
        if (validationResult != null) return validationResult;

        try
        {
            await _computersService.UpdateComputer(computer, model);
            return ApiResponseHelper.Ok(ApplicationErrors.SuccessfullyUpdated);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet("get")]
    public async Task<ActionResult> GetComputers([FromQuery] PaginationModel pagination)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        try
        {
            var items = await _computersService.GetPaginatedResults(pagination);
            return Ok(items);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteComputer(long id)
    {
        var computer = await _dbContext.Computers.FindAsync(id);
        if (computer == null)
            return ApiResponseHelper.NotFound(ApplicationErrors.NotFound);

        _dbContext.Computers.Remove(computer);
        
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
