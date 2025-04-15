using EquipmentInventory.API.Data;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace EquipmentInventory.API.Configurations;

public static class ExceptionHandlerConfiguration
{
    public static void UseCustomStatusCodeHandling(this IApplicationBuilder app)
    {
        app.UseStatusCodePages(async context =>
        {
            context.HttpContext.Response.ContentType = "application/json";

            var response = context.HttpContext.Response.StatusCode switch
            {
                401 => ApiResponse.Unauthorized(ApplicationErrors.Unauthorized),
                403 => ApiResponse.Forbidden(ApplicationErrors.Forbidden),
                _ => null
            };

            if (response != null)
            {
                await context.HttpContext.Response.WriteAsync(
                    JsonSerializer.Serialize(response)
                );
            }
        });
    }

    public static void UseExceptionHandlerService(
        this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = exceptionHandlerFeature?.Error;

                context.Response.ContentType = "application/json";

                // Обработка исключений авторизации
                if (exception is UnauthorizedAccessException)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync(
                        JsonSerializer.Serialize(ApiResponse.Unauthorized(ApplicationErrors.Unauthorized))
                    );
                    return;
                }

                // Обработка других исключений
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(ApiResponse.BadRequest(ApplicationErrors.ServerError))
                );
            });
        });
    }
}
