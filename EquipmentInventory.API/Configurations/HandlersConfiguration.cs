using EquipmentInventory.API.Data.Interfaces;
using EquipmentInventory.API.Handlers;

namespace EquipmentInventory.API.Configurations;

public static class HandlersConfiguration
{
    public static void AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<ICodeHandler, ResetAdminPasswordHandler>();
    }
}
