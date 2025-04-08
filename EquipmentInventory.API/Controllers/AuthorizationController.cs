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

        public AuthorizationController(EquipmentInventoryDbContext dbContext, AuthorizationHelper authorizationHelper)
        {
            _dbContext = dbContext;
            _authorizationHelper = authorizationHelper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _dbContext.Users
                .Include(u => u.IdRoleNavigation)
                .FirstOrDefaultAsync(u => u.Login == model.Login);

            if (user == null || !_authorizationHelper.VerifyPassword(model.Password, user.Password))
                return Unauthorized();

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
                return BadRequest(new { Error = "Имя пользователя уже занято" });

            var passwordHash = _authorizationHelper.HashPassword(model.Password);

            var defaultRole = await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.Name == "Бухгалтер");

            if (defaultRole == null)
                return BadRequest(new { Error = "Роль по умолчанию не найдена" });

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
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Ошибка при создании пользователя", Details = ex.Message });
            }

            return Ok(new { Message = "Пользователь успешно создан" });
        }
    }
}
