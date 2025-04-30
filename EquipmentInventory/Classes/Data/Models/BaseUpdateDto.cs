namespace EquipmentInventory.Classes.Data.Models;

public class BaseUpdateDto
{
    public string? Name { get; set; }

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Name);
    }
}
