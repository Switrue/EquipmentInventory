using DataAccess.Postgres.Migration;
using EquipmentInventory.API.Data;
using EquipmentInventory.API.Data.Interfaces;
using EquipmentInventory.API.Data.Models;
using EquipmentInventory.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EquipmentInventory.API.Handlers;

public class ResetAdminPasswordHandler : ICodeHandler
{
    private readonly EquipmentInventoryDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly HashPasswordHelper _hashPasswordHelper;

    public ResetAdminPasswordHandler(
        EquipmentInventoryDbContext dbContext,
        IConfiguration configuration,
        HashPasswordHelper hashPasswordHelper)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _hashPasswordHelper = hashPasswordHelper;
    }

    public bool CanHandle(string code)
    {
        return code == _configuration["Codes:ResetAdminPassword:Code"]; ;
    }

    public async Task<ActionResult> Handle(BaseModel model)
    {
        try
        {
            var newPassword = _configuration["Codes:ResetAdminPassword:DefaultValue"] 
                ?? throw new Exception("DefaultValue is empty");

            var admins = await _dbContext.Users
                .Include(u => u.IdRoleNavigation)
                .Where(u => u.IdRoleNavigation.Name == RoleNames.Admin)
                .AsTracking()
                .ToListAsync();

            foreach (var admin in admins)
            {
                admin.Password = _hashPasswordHelper.HashPassword(newPassword);
            }

            await _dbContext.SaveChangesAsync();

            return new OkObjectResult(ApiResponse.Ok(ApplicationErrors.AdminPasswordReset));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating passwords: {ex.Message}");
            return new BadRequestObjectResult(ApiResponse.BadRequest(ApplicationErrors.UpdateError));
        }
    }
}
