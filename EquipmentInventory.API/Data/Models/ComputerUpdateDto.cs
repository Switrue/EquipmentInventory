using System.ComponentModel.DataAnnotations;

namespace EquipmentInventory.API.Data.Models;

public class ComputerUpdateDto
{
    public int? Number { get; set; }

    [StringLength(200)]
    public string? PowerSupply { get; set; }

    [StringLength(200)]
    public string? Motherboard { get; set; }

    [StringLength(200)]
    public string? Ram { get; set; }

    [StringLength(200)]
    public string? Cpu { get; set; }

    [StringLength(200)]
    public string? Os { get; set; }
    public string? VideoCard { get; set; }
}
