using System;

namespace EquipmentInventory.Classes.Data.Models;

public class ArchiveDto
{
    public long Id { get; set; }
    public string TypeTechniqueName { get; set; }
    public string MemberName { get; set; }
    public string OfficeNumber { get; set; }
    public string TechniqueName { get; set; }
    public int? ComputerNumber { get; set; }
    public DateTime DateOfPurchase { get; set; }
    public DateTime WriteOffDate { get; set; }
    public DateTime DateOfManufacture { get; set; }
    public bool UnderRepair { get; set; }
    public string Number { get; set; }
    public string Supplier { get; set; }
    public float Cost { get; set; }
    public DateTime? DateOfUse { get; set; }
}
