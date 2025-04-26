namespace EquipmentInventory.Classes.Data.Models;

public class ComputerDto
{
    public long Id { get; set; }
    public int Number { get; set; }
    public string PowerSupply { get; set; }
    public string Motherboard { get; set; }
    public string Ram { get; set; }
    public string Cpu { get; set; }
    public string Os { get; set; }
    public string? VideoCard { get; set; }
}
