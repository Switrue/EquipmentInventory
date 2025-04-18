using DataAccess.Postgres.Migration;
using DataAccess.Postgres.Migration.Models;
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
public class AuthorizationController : ControllerBase
{
    private readonly EquipmentInventoryDbContext _dbContext;
    private readonly AuthorizationService _authorizationHelper;
    private readonly HashPasswordHelper _hashPasswordHelper;

    public AuthorizationController(
        EquipmentInventoryDbContext dbContext, 
        AuthorizationService authorizationHelper,
        HashPasswordHelper hashPasswordHelper)
    {
        _dbContext = dbContext;
        _authorizationHelper = authorizationHelper;
        _hashPasswordHelper = hashPasswordHelper;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        var user = await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.IdRoleNavigation)
            .FirstOrDefaultAsync(u => u.Login == model.Login);

        if (user == null || !_hashPasswordHelper.VerifyPassword(model.Password, user.Password))
            return ApiResponseHelper.Unauthorized(ApplicationErrors.InvalidLoginOrPassword);

        var token = _authorizationHelper.GenerateJwtToken(user);

        return Ok(new { Token = token });
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserAddDto model)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.ValidationError(ModelState);

        if (await _dbContext.Users.AnyAsync(u => u.Login == model.Login))
            return ApiResponseHelper.BadRequest(ApplicationErrors.LoginAlreadyInUse);

        var passwordHash = _hashPasswordHelper.HashPassword(model.Password);

        var defaultRole = await _dbContext.Roles
            .FirstOrDefaultAsync(r => r.Name == RoleNames.Accountant);
        if (defaultRole == null)
            return ApiResponseHelper.BadRequest(ApplicationErrors.RoleNotFound);

        var user = new User
        {
            Username = model.Username,
            Surname = model.Surname,
            Login = model.Login,
            Password = passwordHash,
            IdRole = defaultRole.Id,
            Image = model.Image,
        };

        try
        {
            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.CreationError);
        }

        return ApiResponseHelper.Ok(ApplicationErrors.UserCreated);
    }

    [Authorize]
    [HttpGet("refresh")]
    public async Task<ActionResult> UpdateToken()
    {
        if (!User.TryGetUserId(out var userId))
            return ApiResponseHelper.Unauthorized(ApplicationErrors.AuthenticationError);

        var user = await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.IdRoleNavigation)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null) 
            return ApiResponseHelper.NotFound(ApplicationErrors.UserNotFound);

        var token = _authorizationHelper.GenerateJwtToken(user);

        return Ok(new { Token = token });
    }

    [Authorize]
    [HttpHead]
    public ActionResult Authorize() => Ok();
}
