using EquipmentInventory.API.Data;
using EquipmentInventory.API.Data.Interfaces;
using EquipmentInventory.API.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentInventory.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SettingsController : ControllerBase
{
    [Authorize]
    [HttpPost("code")]
    public async Task<ActionResult> UseCodes([FromBody] BaseModel model)
    {
        if (model is null)
            return BadRequest(ApiResponse.BadRequest(ApplicationErrors.EmptyError));

        var handlers = HttpContext.RequestServices.GetServices<ICodeHandler>();

        foreach (var handler in handlers)
        {
            if (handler.CanHandle(model.Content))
            {
                return await handler.Handle(model);
            }
        }

        return BadRequest(ApiResponse.BadRequest(ApplicationErrors.InvalidCode));
    }
}
