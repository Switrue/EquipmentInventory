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
    public static async Task<PaginatedResult<TechniqueDto>> GetTechniqueWithFiltration(
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

    public static async Task<PaginatedResult<TechniqueDto>> GetTechniqueOutdated(
        PaginationModel pagination,
        int? filter)
    {
        var filterQueryString = filter != null
            ? string.Join("&", $"filter={Uri.EscapeDataString(filter.ToString())}")
            : string.Empty;

        return await ApiClientHelper.GetPaginatedAsync<TechniqueDto>(
            $"api/Technique/outdated?{filterQueryString}&page={pagination.Page}&pageSize={pagination.PageSize}",
            error => CustomMessageBoxHelper.Show(Strings.Error, error));
    }

    public static async Task<BaseResponse> Delete(long id)
    {
        return await ApiClientHelper.DeleteAsync<BaseResponse>(
            $"api/Technique/delete/{id}",
            error => CustomMessageBoxHelper.Show(Strings.Error, error));
    }

    public static async Task<BaseResponse> Add(TechniqueUpdateDto model)
    {
        return await ApiClientHelper.PostAsync<TechniqueUpdateDto, BaseResponse>(
            "/api/Technique/add",
            model,
            error => CustomMessageBoxHelper.Show(Strings.Error, error));
    }

    public static async Task<BaseResponse> Update(
        TechniqueUpdateDto model,
        long id)
    {
        var addParams = QueryParamsHelper.ToQueryParams(model);

        var addQueryString = string.Join("&", addParams
            .Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}")); 

        return await ApiClientHelper.PatchAsync<TechniqueUpdateDto, BaseResponse>(
            $"api/Technique/update/{id}?{addQueryString}",
            model,
            error => CustomMessageBoxHelper.Show(Strings.Error, error));
    }
}
    