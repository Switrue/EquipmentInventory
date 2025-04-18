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
public class MembersController : ControllerBase
{
    private readonly EquipmentInventoryDbContext _dbContext;
    private readonly MembersService _membersService;

    public MembersController(
        EquipmentInventoryDbContext dbContext,
        MembersService membersService)
    {
        _dbContext = dbContext;
        _membersService = membersService;
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPost("add")]
    public async Task<ActionResult> AddMember([FromBody] MemberAddDto model)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        var validationResult = await _membersService.ValidateAddModelAsync(model);
        if (validationResult != null) return validationResult;

        try
        {
            var member = await _membersService.CreateMemberAsync(model);
            return ApiResponseHelper.CreatedAt(nameof(GetMembers), null, ApplicationErrors.SuccessfullyAdded);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPatch("update/{id}")]
    public async Task<ActionResult> UpdateMember(long id, [FromQuery] MemberUpdateDto model)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        var member = await _dbContext.Members.FindAsync(id);
        if (member == null)
            return ApiResponseHelper.NotFound(ApplicationErrors.NotFound);

        var validationResult = await _membersService.ValidateUpdateModelAsync(model);
        if (validationResult != null) return validationResult;

        try
        {
            await _membersService.UpdateMember(member, model);
            return ApiResponseHelper.Ok(ApplicationErrors.SuccessfullyUpdated);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet("get")]
    public async Task<ActionResult> GetMembers(
        [FromQuery] PaginationModel pagination)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        try
        {
            var items = await _membersService.GetPaginatedResults(pagination);
            return Ok(items);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteMember(long id)
    {
        var member = await _dbContext.Members.FindAsync(id);
        if (member == null)
            return ApiResponseHelper.NotFound(ApplicationErrors.NotFound);

        _dbContext.Members.Remove(member);

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
