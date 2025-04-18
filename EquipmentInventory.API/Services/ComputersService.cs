using DataAccess.Postgres.Migration;
using DataAccess.Postgres.Migration.Models;
using EquipmentInventory.API.Data;
using EquipmentInventory.API.Data.Models;
using EquipmentInventory.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EquipmentInventory.API.Services;

public class ComputersService
{
    private readonly EquipmentInventoryDbContext _context;

    public ComputersService(EquipmentInventoryDbContext context)
        => _context = context;

    #region Get
    private IQueryable<Computer> GetQuery()
    {
        return _context.Computers
            .AsNoTracking()
            .OrderBy(c => c.Id);
    }

    public async Task<IEnumerable<ComputerDto>> GetPaginatedResults(PaginationModel pagination)
    {
        var query = GetQuery();

        if (pagination.Page.HasValue && pagination.PageSize.HasValue)
        {
            query = query
                .Skip((pagination.Page.Value - 1) * pagination.PageSize.Value)
                .Take(pagination.PageSize.Value);
        }

        var items = await GetQuery()
            .Select(c => new ComputerDto(
                c.Id,
                c.Number,
                c.PowerSupply,
                c.Motherboard,
                c.Ram,
                c.Cpu,
                c.Os,
                c.VideoCard
            ))
            .ToListAsync();

        return items;
    }
    #endregion

    #region Add
    public async Task<Computer> CreateComputerAsync(ComputerAddDto model)
    {
        var computer = new Computer
        {
            Number = model.Number,
            PowerSupply = model.PowerSupply,
            Motherboard = model.Motherboard,
            Ram = model.Ram,
            Cpu = model.Cpu,
            Os = model.Os,
            VideoCard = model?.VideoCard
        };

        _context.Computers.Add(computer);
        await _context.SaveChangesAsync();
        return computer;
    }
    #endregion

    #region Update
    public void UpdateComputer(Computer existing, ComputerUpdateDto model)
    {
        existing.Number = model.Number ?? existing.Number;
        existing.PowerSupply = model.PowerSupply ?? existing.PowerSupply;
        existing.Motherboard = model.Motherboard ?? existing.Motherboard;
        existing.Ram = model.Ram ?? existing.Ram;
        existing.Cpu = model.Cpu ?? existing.Cpu;
        existing.Os = model.Os ?? existing.Os;
        existing.VideoCard = model.VideoCard ?? existing.VideoCard;
    }
    #endregion

    #region Validation Helpers
    public async Task<ActionResult> ValidateUpdateModelAsync(ComputerUpdateDto model, Computer computer)
    {
        if (model.Number != null && !await IsNumberUniqueAsync(model.Number.Value, computer.Id))
            return ApiResponseHelper.BadRequest(ApplicationErrors.UniqueNumber);

        return null;
    }

    public async Task<ActionResult> ValidateAddModelAsync(ComputerAddDto model)
    {
        if (!await IsNumberUniqueAsync(model.Number))
            return ApiResponseHelper.BadRequest(ApplicationErrors.UniqueNumber);

        return null;
    }

    private async Task<bool> IsNumberUniqueAsync(int number, long? excludeId = null)
            => !await _context.Computers
                .AnyAsync(t => t.Number == number && (excludeId == null || t.Id != excludeId));
    #endregion
}
