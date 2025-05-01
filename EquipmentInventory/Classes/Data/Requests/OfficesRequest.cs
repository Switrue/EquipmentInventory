using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Helpers;
using EquipmentInventory.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Data.Requests;

public static class OfficesRequest
{
    public static async Task<List<OfficeDto>> Get()
    {
        return await ApiClientHelper.GetAsync<List<OfficeDto>>(
            "api/Offices/get",
            Console.WriteLine);
    }

    public static async Task<ObservableCollection<OfficeDto>> GetOfficesItemsAsync()
    {
        var result = await Get();
        return result is null
            ? new ObservableCollection<OfficeDto>()
            : new ObservableCollection<OfficeDto>(result);
    }

    public static async Task<List<object>> GetOfficesDataAsync()
    {
        var items = await Get();
        return items.Select(m => new
        {
            m.Id,
            Номер = m.Number,
            Этаж = m.Floor,
            Наименование = m.Name
        }).ToList<object>();
    }

    public static async Task<BaseResponse> Delete(long id)
    {
        return await ApiClientHelper.DeleteAsync<BaseResponse>(
            $"/api/Offices/delete/{id}",
            error => CustomMessageBoxHelper.Show(Strings.Error, error));
    }

    public static async Task<BaseResponse> Add(OfficeUpdateDto model)
    {
        return await ApiClientHelper.PostAsync<OfficeUpdateDto, BaseResponse>(
            "/api/Offices/add",
            model,
            error => CustomMessageBoxHelper.Show(Strings.Error, error));
    }

    public static async Task<BaseResponse> Update(long id, OfficeUpdateDto model)
    {
        var filterParams = QueryParamsHelper.ToQueryParams(model);

        var filterQueryString = string.Join("&", filterParams
            .Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));

        return await ApiClientHelper.PatchAsync<OfficeUpdateDto, BaseResponse>(
            $"/api/Offices/update/{id}?{filterQueryString}",
            model,
            error => CustomMessageBoxHelper.Show(Strings.Error, error));
    }
}
