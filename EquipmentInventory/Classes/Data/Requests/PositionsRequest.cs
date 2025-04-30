using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Helpers;
using EquipmentInventory.Properties;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Data.Requests;

public static class PositionsRequest
{
    public static async Task<List<BaseDto>> Get()
    {
        return await ApiClientHelper.GetAsync<List<BaseDto>>(
            "/api/Positions/get",
            error => CustomMessageBoxHelper.Show(Strings.Error, error));
    }

    public static async Task<ObservableCollection<BaseDto>> GetPositionsItemsAsync()
    {
        var result = await Get();
        return result is null
            ? new ObservableCollection<BaseDto>()
            : new ObservableCollection<BaseDto>(result);
    }

    public static async Task<List<object>> GetPositionsDataAsync()
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
            $"/api/Positions/delete/{id}",
            error => CustomMessageBoxHelper.Show(Strings.Error, error));
    }

    public static async Task<BaseResponse> Add(BaseUpdateDto model)
    {
        return await ApiClientHelper.PostAsync<BaseUpdateDto, BaseResponse>(
            "/api/Positions/add",
            model,
            error => CustomMessageBoxHelper.Show(Strings.Error, error));
    }

    public static async Task<BaseResponse> Update(long id, BaseUpdateDto model)
    {
        return await ApiClientHelper.PatchAsync<BaseUpdateDto, BaseResponse>(
            $"/api/Positions/update/{id}?Name={model.Name}",
            model,
            error => CustomMessageBoxHelper.Show(Strings.Error, error));
    }
}
