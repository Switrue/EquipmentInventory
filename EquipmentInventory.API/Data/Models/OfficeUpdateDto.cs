using System.ComponentModel.DataAnnotations;

namespace EquipmentInventory.API.Data.Models;

public class OfficeUpdateDto
{
    public int? Floor { get; set; }
    public int? Number { get; set; }

    [StringLength(60)]
    public string? Name { get; set; }
}
