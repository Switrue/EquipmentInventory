namespace EquipmentInventory.Classes.Data.Models;

public class ComputerUpdateDto
{
    public int? Number { get; set; }
    public string? Motherboard { get; set; }
    public string? Cpu { get; set; }
    public string? Ram { get; set; }
    public string? Os { get; set; }
    public string? PowerSupply { get; set; }
    public string? VideoCard { get; set; }

    public bool IsValid()
    {
        return Number.HasValue &&
               !string.IsNullOrWhiteSpace(PowerSupply) &&
               !string.IsNullOrWhiteSpace(Motherboard) &&
               !string.IsNullOrWhiteSpace(Ram) &&
               !string.IsNullOrWhiteSpace(Cpu) &&
               !string.IsNullOrWhiteSpace(Os);
    }
}
