using DataAccess.Postgres.Migration;
using EquipmentInventory.API.Data;
using EquipmentInventory.API.Data.Models;
using EquipmentInventory.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EquipmentInventory.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArchiveController : ControllerBase
{
    private readonly EquipmentInventoryDbContext _dbContext;

    public ArchiveController(EquipmentInventoryDbContext dbContext)
        => _dbContext = dbContext;

    [Authorize]
    [HttpGet("get")]
    public async Task<ActionResult> GetArchive(
        [FromQuery] PaginationModel pagination)
    {
        var query = _dbContext.Archives
            .AsNoTracking()
            .OrderBy(a => a.Id);

        try
        {
            if (pagination.Page.HasValue && pagination.PageSize.HasValue)
            {
                var items = await query
                    .Skip((pagination.Page.Value - 1) * pagination.PageSize.Value)
                    .Take(pagination.PageSize.Value)
                    .ToListAsync();

                return Ok(items);
            }
            else
            {
                var items = await query.ToListAsync();
                return Ok(items);
            }
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }
}
