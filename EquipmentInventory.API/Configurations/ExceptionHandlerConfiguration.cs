using EquipmentInventory.API.Data;
using System.Text.Json;

namespace EquipmentInventory.API.Configurations;

public static class ExceptionHandlerConfiguration
{
    public static void UseExceptionHandlerService(
        this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(ApiResponse.BadRequest(ApplicationErrors.ServerError)));
            });
        });
    }
}
