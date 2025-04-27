using DataAccess.Postgres.Migration;
using EquipmentInventory.API.Data;
using EquipmentInventory.API.Data.Interfaces;
using EquipmentInventory.API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EquipmentInventory.API.Handlers;

public class ResetArchive : ICodeHandler
{
    private readonly EquipmentInventoryDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public ResetArchive(
        EquipmentInventoryDbContext dbContext,
        IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public bool CanHandle(string code)
        => code == _configuration["Codes:ResetArchive:Code"];

    public async Task<ActionResult> Handle(BaseModel model)
    {
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                await _dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE archive");
                await _dbContext.Database.ExecuteSqlRawAsync("ALTER SEQUENCE public.archive_id_archive_seq RESTART WITH 1");

                await transaction.CommitAsync();

                return new OkObjectResult(ApiResponse.Ok(ApplicationErrors.ArchiveReset));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when deleting archive records: {ex.Message}");
                return new BadRequestObjectResult(ApiResponse.BadRequest(ApplicationErrors.UpdateError));
            }
        }
    }
}
