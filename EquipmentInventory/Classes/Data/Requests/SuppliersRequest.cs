using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Data.Requests;

public static class SuppliersRequest
{
    public static async Task<List<BaseDto>> Get()
    {
        return await ApiClientHelper.GetAsync<List<BaseDto>>(
            "/api/Suppliers/get",
            Console.WriteLine);
    }

    public static async Task<ObservableCollection<BaseDto>> GetSuppliersItems()
    {
        var result = await Get();
        return result is null
            ? new ObservableCollection<BaseDto>()
            : new ObservableCollection<BaseDto>(result);
    }
}
