using DataAccess.Postgres.Migration;
using EquipmentInventory.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EquipmentInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly EquipmentInventoryDbContext _dbContext;

        public UsersController(EquipmentInventoryDbContext dbContext)
        {
            _dbContext = dbContext; 
        }

        [Authorize(Roles = RoleNames.Admin)]
        [HttpGet("user")]
        public async Task<ActionResult<object>> GetUser(long userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.IdRoleNavigation)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) 
                return NotFound(new { Message = "Пользователь не найден" });

            var result = new
            {
                user.Id,
                user.Username,
                user.Surname,
                user.Image,
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
                role = user.IdRoleNavigation?.Name ?? "Unknown",
                user.Image
            });

            return Ok(result);
        }

        [Authorize]
        [HttpGet("image")]
        public async Task<ActionResult> GetUserImage()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            return Ok(new { Message = user?.Image });
        }
    }
}
