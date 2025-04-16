using System.ComponentModel.DataAnnotations;

namespace EquipmentInventory.API.Data.Models;

public class TechniqueUpdateModel
{
    [StringLength(30)]
    public string? Number { get; set; }

    [Range(1, long.MaxValue)]
    public long? IdTypeTechnique { get; set; }

    [StringLength(100)]
    public string? Name { get; set; }

    [Range(0, long.MaxValue)]
    public long? IdMember { get; set; }

    [Range(0, long.MaxValue)]
    public long? IdOffice { get; set; }

    [Range(0, long.MaxValue)]
    public long? IdComputer { get; set; }

    public DateOnly? DateOfPurchase { get; set; }
    public DateOnly? DateOfManufacture { get; set; }
    public DateOnly? DateOfUse { get; set; }

    [Range(1, long.MaxValue)]
    public long? IdSupplier { get; set; }

    [Range(0, float.MaxValue)]
    public float? Cost { get; set; }

    public bool? UnderRepair { get; set; }
}
