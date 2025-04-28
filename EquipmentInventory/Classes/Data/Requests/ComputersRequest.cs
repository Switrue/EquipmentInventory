using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Data.Requests;

public static class ComputersRequest
{
    public static async Task<List<ComputerDto>> Get()
    {
        return await ApiClientHelper.GetAsync<List<ComputerDto>>(
            "/api/Computers/get",
            Console.WriteLine);
    }

    public static async Task<ObservableCollection<ComputerDto>> GetComputersItemsAsync()
    {
        var result = await Get();
        return result is null
            ? new ObservableCollection<ComputerDto>() 
            : new ObservableCollection<ComputerDto>(result);

    }

    public static async Task<List<object>> GetComputersDataAsync()
    {
        var items = await Get();
        return items.Select(m => new
        {
            m.Id,
            Номер = m.Number,
            Материнскаяㅤплата = m.Motherboard,
            Процессор = m.Cpu,
            Оперативнаяㅤпамять = m.Ram,
            Операционнаяㅤсистема = m.Os,
            Блокㅤпитания = m.PowerSupply,
            Видеокарта = m.VideoCard ?? "-",
        }).ToList<object>();
    }
}
