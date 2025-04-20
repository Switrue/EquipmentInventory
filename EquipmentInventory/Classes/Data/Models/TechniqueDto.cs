using System;

namespace EquipmentInventory.Classes.Data.Models;

public class TechniqueDto
{
    public long Id { get; set; }
    public string Number { get; set; }
    public string TypeTechnique { get; set; }
    public string Name { get; set; }
    public string? Member { get; set; }
    public int? Office { get; set; }
    public int? Computer { get; set; }
    public DateTime DateOfPurchase { get; set; }
    public DateTime DateOfManufacture { get; set; }
    public DateTime? DateOfUse { get; set; }
    public string Supplier { get; set; }
    public float Cost { get; set; }
    public bool UnderRepair { get; set; }
}