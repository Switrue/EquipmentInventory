namespace EquipmentInventory.API.Data.Models;

public class TechniqueFiltersDto : TechniqueUpdateDto
{
    public bool? IsFastened { get; set; }
    public bool? IsUnderRepair { get; set; }
}
