using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public static async Task<ObservableCollection<OfficeDto>> GetOfficesItems()
    {
        var result = await Get();
        return result is null
            ? new ObservableCollection<OfficeDto>()
            : new ObservableCollection<OfficeDto>(result);
    }
}
