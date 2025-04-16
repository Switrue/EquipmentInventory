using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EquipmentInventory.API.Data;

public class ApiResponse
{
    private static object Create(string message, object? data = null)
    {
        return data == null
            ? new { Message = message }
            : new { Message = message, Data = data };
    }

    public static object Error(ModelStateDictionary modelState)
    {
        var errors = modelState
            .Where(entry => entry.Value.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );
        return Create("Произошла одна или несколько ошибок проверки", errors);
    }

    public static object NotFound(string message) => Create(message);
    public static object BadRequest(string message) => Create(message);
    public static object Unauthorized(string message) => Create(message);
    public static object Forbidden(string message) => Create(message);
    public static object Ok(string message, object? data = null) => Create(message, data);
}
