namespace EquipmentInventory.API.Data.Models;

public record ComputerDto(
    long Id,
    int Number,
    string PowerSupply,
    string Motherboard,
    string Ram,
    string Cpu,
    string Os,
    string? VideoCard
);
