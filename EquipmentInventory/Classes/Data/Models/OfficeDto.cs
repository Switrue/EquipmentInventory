namespace EquipmentInventory.Classes.Data.Models;

public class OfficeDto
{
    public long Id { get; set; }
    public int Floor { get; set; }
    public int Number { get; set; }
    public string Name { get; set; }

    public string DisplayData => $"{Number} | {Name}";
}
