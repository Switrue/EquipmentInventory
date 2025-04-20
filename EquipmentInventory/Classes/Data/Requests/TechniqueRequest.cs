using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Helpers;
using EquipmentInventory.Properties;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Data.Requests;

public static class TechniqueRequest
{
    public static async Task<PaginatedResult<TechniqueDto>> GetTechnique(
        PaginationModel pagination, 
        TechniqueFiltersDto filter)
    {
        var filterParams = QueryParamsHelper.ToQueryParams(filter);
        
        var filterQueryString = string.Join("&", filterParams
            .Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));

        return await ApiClientHelper.GetPaginatedAsync<TechniqueDto>(
            $"api/Technique/get?{filterQueryString}&page={pagination.Page}&pageSize={pagination.PageSize}", 
            error => CustomMessageBoxHelper.Show(Strings.Error, error));
    }
}
    