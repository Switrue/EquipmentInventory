using System.ComponentModel.DataAnnotations;

namespace EquipmentInventory.API.Data.Models;

public class PaginationModel
{
    [Range(1, int.MaxValue, ErrorMessage = ApplicationErrors.InvalidPage)]
    public int? Page { get; set; }

    [Range(1, 100, ErrorMessage = ApplicationErrors.InvalidPageSize)]
    public int? PageSize { get; set; }
}
