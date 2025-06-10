using DataAccess.Postgres.Migration;
using Microsoft.EntityFrameworkCore;

namespace EquipmentInventory.API.Configurations;

public static class DbContextConfiguration
{
    public static void AddDbContextConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EquipmentInventoryDbContext>(options =>
        {
            options
                .UseNpgsql(configuration.GetConnectionString(nameof(EquipmentInventoryDbContext)))
                .EnableSensitiveDataLogging(false)
                .LogTo(Console.WriteLine, LogLevel.Information);
        });
    }
}
