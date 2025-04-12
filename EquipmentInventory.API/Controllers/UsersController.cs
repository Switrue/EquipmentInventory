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
    public class UsersController : ControllerBase
    {
        private readonly EquipmentInventoryDbContext _dbContext;
        private readonly AuthorizationHelper _authorizationHelper;

        public UsersController(EquipmentInventoryDbContext dbContext, AuthorizationHelper authorizationHelper)
        {
            _dbContext = dbContext;
            _authorizationHelper = authorizationHelper;
        }

        [Authorize]
        [HttpPost("updateProfile")]
        public async Task<ActionResult> UpdateUser([FromBody] UpdateUser model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Errors = ModelState });

            if (!User.TryGetUserId(out var userId))
                return Unauthorized();

            var user = await _dbContext.Users
                .FindAsync(userId);
            if (user is null)
                return NotFound(new { Message = "Пользователь не найден" });

            UpdateUserFields(user, model);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                return BadRequest(new { Message = "Конфликт при обновлении" });
            }

            return Ok(new { Message = "Данные обновлены" });
        }

        [Authorize(Roles = RoleNames.Admin)]
        [HttpGet("user")]
        public async Task<ActionResult<object>> GetUser(long userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.IdRoleNavigation)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) 
                return NotFound(new { Message = "Пользователь не найден" });

            var result = new
            {
                user.Id,
                user.Username,
                user.Surname,
                role = user.IdRoleNavigation?.Name ?? "Unknown"
            };

            return Ok(result);
        }

        [Authorize]
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            var users = await _dbContext.Users
                .Include(u => u.IdRoleNavigation)
                .ToListAsync();

            var result = users.Select(user => new
            {
                user.Id,
                user.Username,
                user.Surname,
                role = user.IdRoleNavigation?.Name ?? "Unknown"
            });

            return Ok(result);
        }

        [Authorize]
        [HttpGet("image")]
        public async Task<ActionResult> GetUserImage()
        {
            if (!User.TryGetUserId(out var userId))
                return Unauthorized();

            var user = await _dbContext.Users
                .FindAsync(userId);
            if (user is null)
                return NotFound(new { Message = "Пользователь не найден" });

            return Ok(new { Message = user?.Image });
        }

        private void UpdateUserFields(User user, UpdateUser model)
        {
            if (!string.IsNullOrWhiteSpace(model.Username)) user.Username = model.Username;
            if (!string.IsNullOrWhiteSpace(model.Surname)) user.Surname = model.Surname;
            if (model.Image != null) user.Image = model.Image;

            if (!string.IsNullOrWhiteSpace(model.Password))
                user.Password = _authorizationHelper.HashPassword(model.Password);
        }
    }
}
