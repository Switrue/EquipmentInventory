using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helpers;
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
}
