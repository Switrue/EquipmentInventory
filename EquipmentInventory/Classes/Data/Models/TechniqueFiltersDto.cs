using System;

namespace EquipmentInventory.Classes.Data.Models;

public class TechniqueFiltersDto
{
    public string? Number { get; set; }
    public string? TypeTechnique { get; set; }
    public string? Name { get; set; }
    public string? Member { get; set; }
    public int? Office { get; set; }
    public int? Computer { get; set; }
    public DateTime? DateOfPurchase { get; set; }
    public DateTime? DateOfManufacture { get; set; }
    public DateTime? DateOfUse { get; set; }
    public string? Supplier { get; set; }
    public float? FromCost { get; set; }
    public float? UpToCost { get; set; }
    public bool? IsFastened { get; set; }
    public bool? IsUnderRepair { get; set; }
}
