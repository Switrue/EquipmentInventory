using EquipmentInventory.API.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentInventory.API.Data.Interfaces;

public interface ICodeHandler
{
    bool CanHandle(string code);
    Task<ActionResult> Handle(BaseModel model);
}
