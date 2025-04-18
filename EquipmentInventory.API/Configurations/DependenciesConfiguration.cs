using EquipmentInventory.API.Data.Interfaces;
using EquipmentInventory.API.Handlers;
using EquipmentInventory.API.Helpers;
using EquipmentInventory.API.Services;

namespace EquipmentInventory.API.Configurations;

public static class DependenciesConfiguration
{
    public static void AddDependencies(this IServiceCollection services)
    {
        // Handlers
        services.AddScoped<ICodeHandler, ResetAdminPasswordHandler>();

        // Helpers
        services.AddScoped<EntityValidator>();
        services.AddScoped<HashPasswordHelper>();

        // Servises
        services.AddScoped<AuthorizationService>();
        services.AddScoped<ComputersService>();
        services.AddScoped<TechniqueService>();
        services.AddScoped<MembersService>();
    }
}
