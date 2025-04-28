using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Helpers;
using EquipmentInventory.Properties;
using System.Collections.Generic;
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

    public static async Task<List<object>> GetPositionsDataAsync()
    {
        var items = await Get();
        return items.Select(m => new
        {
            m.Id,
            Наименование = m.Name
        }).ToList<object>();
    }
}
