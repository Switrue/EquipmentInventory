using EquipmentInventory.API.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentInventory.API.Helpers;

public static class ApiResponseHelper
{
    public static CreatedAtActionResult CreatedAt(string actionName, object value, string message)
    {
        return new CreatedAtActionResult(
            actionName,
            controllerName: null,
            routeValues: null,
            value: ApiResponse.Ok(message));
    }

    public static BadRequestObjectResult BadRequest(string message)
        => new BadRequestObjectResult(ApiResponse.BadRequest(message));

    public static BadRequestObjectResult ValidationError(ModelStateDictionary modelState)
        => new BadRequestObjectResult(ApiResponse.Error(modelState));

    public static NotFoundObjectResult NotFound(string message)
        => new NotFoundObjectResult(ApiResponse.NotFound(message));

    public static ObjectResult DatabaseError(string message)
        => new ObjectResult(ApiResponse.BadRequest(message))
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
}
