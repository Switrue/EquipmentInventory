using DataAccess.Postgres.Migration;
using DataAccess.Postgres.Migration.Models;
using EquipmentInventory.API.Data;
using EquipmentInventory.API.Data.Models;
using EquipmentInventory.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentInventory.API.Controllers
{
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
        public async Task<ActionResult> UpdateUser([FromBody] UserModel model)
        {
            if (!ModelState.IsValid)
                return ApiResponseHelper.ValidationError(ModelState);

            if (!User.TryGetUserId(out var userId))
                return Unauthorized(ApiResponse.Unauthorized(ApplicationErrors.AuthenticationError));

            var user = await _dbContext.Users
                .FindAsync(userId);
            if (user is null)
                return NotFound(ApiResponse.NotFound(ApplicationErrors.UserNotFound));

            UpdateUserFields(user, model);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                return BadRequest(ApiResponse.BadRequest(ApplicationErrors.UpdateError));
            }

            return Ok(ApiResponse.Ok(ApplicationErrors.SuccessfullyUpdated));
        }

        [Authorize]
        [HttpGet("image-me")]
        public async Task<ActionResult> GetUserImage()
        {
            if (!User.TryGetUserId(out var userId))
                return Unauthorized();

            var user = await _dbContext.Users
                .FindAsync(userId);
            if (user is null)
                return NotFound(ApiResponse.NotFound(ApplicationErrors.UserNotFound));

            return Ok(new { Message = user?.Image });
        }

        private void UpdateUserFields(User user, UserModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Username)) user.Username = model.Username;
            if (!string.IsNullOrWhiteSpace(model.Surname)) user.Surname = model.Surname;
            if (model.Image != null) user.Image = model.Image;

            if (!string.IsNullOrWhiteSpace(model.Password))
                user.Password = _hashPasswordHelper.HashPassword(model.Password);
        }
    }
}
