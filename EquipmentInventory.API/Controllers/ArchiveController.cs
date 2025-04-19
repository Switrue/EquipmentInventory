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
        [FromQuery] PaginationModel pagination,
        [FromQuery] string? filter)
    {
        var query = _dbContext.Archives
            .AsNoTracking();

        if (!string.IsNullOrEmpty(filter))
        {
            var request = filter.ToLower();
            DateOnly parsedDate;
            float parsedFloat;

            bool isDateParsed = DateOnly.TryParse(request, out parsedDate);
            bool isFloatParsed = float.TryParse(request, out parsedFloat);

            query = query.Where(t =>
                t.Number.ToLower().Contains(request) ||
                t.TechniqueName.ToLower().Contains(request) ||
                t.TypeTechniqueName.ToLower().Contains(request) ||
                t.Supplier.ToLower().Contains(request) ||
                (t.MemberName != null && t.MemberName.ToLower().Contains(request)) ||
                (t.OfficeNumber != null && t.OfficeNumber.ToLower().Contains(request)) ||
                (t.ComputerNumber.HasValue && t.ComputerNumber.Value.ToString() == request) ||
                (isFloatParsed && t.Cost == parsedFloat) ||
                (isDateParsed && (
                    t.WriteOffDate == parsedDate || 
                    t.DateOfPurchase == parsedDate || 
                    t.DateOfManufacture == parsedDate || 
                    t.DateOfUse == parsedDate))
            );
        }

        query = query.OrderBy(t => t.Id);

        try
        {
            var items = pagination.Page.HasValue && pagination.PageSize.HasValue
                ? await query.Skip((pagination.Page.Value - 1) * pagination.PageSize.Value)
                             .Take(pagination.PageSize.Value)
                             .ToListAsync()
                : await query.ToListAsync();

            return Ok(items);
        }
        catch
        {
            return ApiResponseHelper.DatabaseError(ApplicationErrors.InternalServerError);
        }
    }
}
