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

public static class TypeTechniqueRequest
{
    public static async Task<List<BaseDto>> Get()
    {
        return await ApiClientHelper.GetAsync<List<BaseDto>>(
            "/api/TypeTechnique/get",
            Console.WriteLine);
    }

    public static async Task<ObservableCollection<BaseDto>> GetTypeTechniqueItemsAsync()
    {
        var result = await Get();
        return result is null
            ? new ObservableCollection<BaseDto>()
            : new ObservableCollection<BaseDto>(result);
    }

    public static async Task<List<object>> GetTypeTechniqueDataAsync()
    {
        var items = await Get();
        return items.Select(m => new
        {
            m.Id,
            Наименование = m.Name
        }).ToList<object>();
    }

    public static async Task<BaseResponse> Delete(long id)
    {
        return await ApiClientHelper.DeleteAsync<BaseResponse>(
            $"/api/TypeTechnique/delete/{id}",
            error => CustomMessageBoxHelper.Show(Strings.Error, error));
    }

    public static async Task<BaseResponse> Add(BaseUpdateDto model)
    {
        return await ApiClientHelper.PostAsync<BaseUpdateDto, BaseResponse>(
            "/api/TypeTechnique/add",
            model,
            error => CustomMessageBoxHelper.Show(Strings.Error, error));
    }

    public static async Task<BaseResponse> Update(long id, BaseUpdateDto model)
    {
        return await ApiClientHelper.PatchAsync<BaseUpdateDto, BaseResponse>(
            $"/api/TypeTechnique/update/{id}?Name={model.Name}",
            model,
            error => CustomMessageBoxHelper.Show(Strings.Error, error));
    }
}
