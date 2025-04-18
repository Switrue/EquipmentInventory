using System.ComponentModel.DataAnnotations;

namespace EquipmentInventory.API.Data.Models;

public class OfficeAddDto
{
    [Required]
    public int Floor { get; set; }

    [Required]
    public int Number { get; set; }

    [Required]
    [StringLength(60)]
    public string Name { get; set; }
}
