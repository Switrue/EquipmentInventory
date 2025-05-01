namespace EquipmentInventory.Classes.Data.Models;

public class OfficeUpdateDto
{
    public int? Number { get; set; }
    public int? Floor { get; set; }
    public string? Name { get; set; }

    public bool IsValid()
    {
        return Floor.HasValue &&
               Number.HasValue &&
               !string.IsNullOrWhiteSpace(Name);
    }
}
