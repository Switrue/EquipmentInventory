namespace EquipmentInventory.API.Data.Models;

public record TechniqueFiltersDto(
    string? Number,
    string? TypeTechnique,
    string? Name,
    string? Member,
    int? Office,
    int? Computer,
    DateOnly? DateOfPurchase,
    DateOnly? DateOfManufacture,
    DateOnly? DateOfUse,
    string? Supplier,
    float? FromCost,
    float? UpToCost,
    bool? IsFastened,
    bool? IsUnderRepair
); 
