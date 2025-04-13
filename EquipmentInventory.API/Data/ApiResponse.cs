namespace EquipmentInventory.API.Data;

public class ApiResponse
{
    private static object Create(string message, object? data = null)
    {
        return data == null
            ? new { Message = message }
            : new { Message = message, Data = data };
    }

    public static object NotFound(string message) => Create(message);
    public static object BadRequest(string message) => Create(message);
    public static object Unauthorized(string message) => Create(message);
    public static object Ok(string message, object? data = null) => Create(message, data);
}
