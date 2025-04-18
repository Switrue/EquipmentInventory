using System.ComponentModel.DataAnnotations;

namespace EquipmentInventory.API.Data.Models;

public class BaseUpdateDto
{
    [StringLength(100)]
    public string? Name { get; set; }
}
