using System.ComponentModel.DataAnnotations;

namespace EquipmentInventory.API.Data.Models;

public class TechniqueAddDto
{
    [Required]
    [StringLength(30)]
    public string Number { get; set; }

    [Required]
    public long IdTypeTechnique { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    public long? IdMember { get; set; }
    public long? IdOffice { get; set; }
    public long? IdComputer { get; set; }

    [Required]
    public DateOnly DateOfPurchase { get; set; }

    [Required]
    public DateOnly DateOfManufacture { get; set; }
    public DateOnly? DateOfUse { get; set; }

    [Required]
    public long IdSupplier { get; set; }

    [Required]
    public float Cost { get; set; }
    public bool? UnderRepair { get; set; } = false;
}
