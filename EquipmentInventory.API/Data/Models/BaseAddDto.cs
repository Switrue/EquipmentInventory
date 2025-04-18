using System.ComponentModel.DataAnnotations;

namespace EquipmentInventory.API.Data.Models;

public class BaseAddDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
}
