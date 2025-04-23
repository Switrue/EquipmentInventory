using DataAccess.Postgres.Migration;
using DataAccess.Postgres.Migration.Models;
using EquipmentInventory.API.Data;
using EquipmentInventory.API.Data.Models;
using EquipmentInventory.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EquipmentInventory.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly EquipmentInventoryDbContext _dbContext;
    private readonly HashPasswordHelper _hashPasswordHelper;

    public UsersController(
        EquipmentInventoryDbContext dbContext,
        HashPasswordHelper hashPasswordHelper)
    {
        _dbContext = dbContext;
        _hashPasswordHelper = hashPasswordHelper;
    }

    [Authorize]
    [HttpPatch("update-me")]
    public async Task<ActionResult> UpdateUser([FromBody] UserUpdateDto model)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        if (!User.TryGetUserId(out var userId))
            return ApiResponseHelper.Unauthorized(ApplicationErrors.AuthenticationError);

        var user = await _dbContext.Users
            .FindAsync(userId);
        if (user is null)
            return ApiResponseHelper.NotFound(ApplicationErrors.UserNotFound);

        UpdateUserFields(user, model);

        try
        {
            await _dbContext.SaveChangesAsync();
            return ApiResponseHelper.Ok(ApplicationErrors.SuccessfullyUpdated);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.UpdateError);
        }
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet("get")]
    public async Task<ActionResult> GetUsers([FromQuery] PaginationModel pagination)
    {
        var query = _dbContext.Users
            .AsNoTracking()
            .Include(u => u.IdRoleNavigation)
            .Where(u => u.IdRoleNavigation.Name != RoleNames.Admin);

        query = query.OrderByDescending(u => u.Id);

        if (pagination.Page.HasValue && pagination.PageSize.HasValue)
        {
            query = query
                .Skip((pagination.Page.Value - 1) * pagination.PageSize.Value)
                .Take(pagination.PageSize.Value);
        }

        var items = await query
            .Select(c => new
            {
                c.Id,
                c.Username,
                c.Surname,
                c.Login,
                c.IdRoleNavigation.Name
            })
            .ToListAsync();

        return Ok(items);
    }

    [Authorize]
    [HttpGet("image-me")]
    public async Task<ActionResult> GetUserImage()
    {
        if (!User.TryGetUserId(out var userId))
            return ApiResponseHelper.Unauthorized(ApplicationErrors.AuthenticationError);

        var user = await _dbContext.Users
            .FindAsync(userId);
        if (user is null)
            return ApiResponseHelper.NotFound(ApplicationErrors.UserNotFound);

        return Ok(new { Message = user?.Image });
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteUser(long id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null)
            return ApiResponseHelper.NotFound(ApplicationErrors.NotFound);

        _dbContext.Users.Remove(user);

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

    private void UpdateUserFields(User user, UserUpdateDto model)
    {
        if (!string.IsNullOrWhiteSpace(model.Username)) user.Username = model.Username;
        if (!string.IsNullOrWhiteSpace(model.Surname)) user.Surname = model.Surname;
        if (model.Image != null) user.Image = model.Image;
        if (!string.IsNullOrWhiteSpace(model.Password))
            user.Password = _hashPasswordHelper.HashPassword(model.Password);
    }
}
