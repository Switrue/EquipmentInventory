using DataAccess.Postgres.Migration;
using DataAccess.Postgres.Migration.Models;
using EquipmentInventory.API.Data;
using EquipmentInventory.API.Data.Models;
using EquipmentInventory.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EquipmentInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly EquipmentInventoryDbContext _dbContext;
        private readonly AuthorizationHelper _authorizationHelper;

        public AuthorizationController(
            EquipmentInventoryDbContext dbContext, 
            AuthorizationHelper authorizationHelper)
        {
            _dbContext = dbContext;
            _authorizationHelper = authorizationHelper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Errors = ModelState });

            var user = await _dbContext.Users
                .Include(u => u.IdRoleNavigation)
                .FirstOrDefaultAsync(u => u.Login == model.Login);

            if (user == null || !_authorizationHelper.VerifyPassword(model.Password, user.Password))
                return Unauthorized(ApiResponse.Unauthorized(ApplicationErrors.InvalidLoginOrPassword));

            var token = _authorizationHelper.GenerateJwtToken(user);

            return Ok(new { Token = token });
        }

        [Authorize(Roles = RoleNames.Admin)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Errors = ModelState });

            if (await _dbContext.Users.AnyAsync(u => u.Login == model.Login))
                return BadRequest(ApiResponse.BadRequest(ApplicationErrors.LoginAlreadyInUse));

            var passwordHash = _authorizationHelper.HashPassword(model.Password);

            var defaultRole = await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.Name == RoleNames.Accountant);
            if (defaultRole == null)
                return BadRequest(ApiResponse.BadRequest(ApplicationErrors.RoleNotFound));

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
                return BadRequest(ApiResponse.BadRequest(ApplicationErrors.CreationError));
            }

            return Ok(ApiResponse.Ok(ApplicationErrors.UserCreated));
        }

        [Authorize]
        [HttpGet("refresh")]
        public async Task<ActionResult> UpdateToken()
        {
            if (!User.TryGetUserId(out var userId))
                return Unauthorized();

            var user = await _dbContext.Users
                .Include(u => u.IdRoleNavigation)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) 
                return NotFound(ApiResponse.NotFound(ApplicationErrors.UserNotFound));

            var token = _authorizationHelper.GenerateJwtToken(user);

            return Ok(new { Token = token });
        }

        [Authorize]
        [HttpGet]
        public ActionResult Authorize() => Ok();
    }
}
