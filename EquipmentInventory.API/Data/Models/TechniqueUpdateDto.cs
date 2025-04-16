using System.ComponentModel.DataAnnotations;

namespace EquipmentInventory.API.Data.Models;

public class TechniqueUpdateDto
{
    [StringLength(30)] public string? Number { get; set; }
    public long? IdTypeTechnique { get; set; }
    [StringLength(100)] public string? Name { get; set; }
    public long? IdMember { get; set; }
    public long? IdOffice { get; set; }
    public long? IdComputer { get; set; }
    public DateOnly? DateOfPurchase { get; set; }
    public DateOnly? DateOfManufacture { get; set; }
    public DateOnly? DateOfUse { get; set; }
    public long? IdSupplier { get; set; }
    public float? Cost { get; set; }
    public bool? UnderRepair { get; set; }
}
