namespace EquipmentInventory.API.Data.Models;

public record TechniqueDto(
    long Id,
    string Number,
    string TypeTechnique,
    string Name,
    string? Member,
    int? Office,
    int? Computer,
    DateOnly DateOfPurchase,
    DateOnly DateOfManufacture,
    DateOnly? DateOfUse,
    string Supplier,
    float Cost,
    bool UnderRepair
);
