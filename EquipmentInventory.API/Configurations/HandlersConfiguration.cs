using EquipmentInventory.API.Data.Interfaces;
using EquipmentInventory.API.Handlers;
using EquipmentInventory.API.Helpers;
using EquipmentInventory.API.Services;

namespace EquipmentInventory.API.Configurations;

public static class HandlersConfiguration
{
    public static void AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<ICodeHandler, ResetAdminPasswordHandler>();

        // Helpers
        services.AddScoped<EntityValidator>();
        services.AddScoped<TechniqueService>();
    }
}
