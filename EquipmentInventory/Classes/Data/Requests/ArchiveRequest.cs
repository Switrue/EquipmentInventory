using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Helpers;
using EquipmentInventory.Properties;
using System;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Data.Requests;

public class ArchiveRequest
{
    public static async Task<PaginatedResult<ArchiveDto>> GetArchive(
        PaginationModel pagination,
        string? filter = null)
    {
        var filterQueryString = !string.IsNullOrWhiteSpace(filter)
            ? string.Join("&", $"filter={Uri.EscapeDataString(filter)}")
            : string.Empty;

        return await ApiClientHelper.GetPaginatedAsync<ArchiveDto>(
            $"api/Archive/get?{filterQueryString}&page={pagination.Page}&pageSize={pagination.PageSize}",
            error => CustomMessageBoxHelper.Show(Strings.Error, error));
    }
}
