using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public static async Task<ObservableCollection<ComputerDto>> GetComputersItems()
    {
        var result = await Get();
        return result is null
            ? new ObservableCollection<ComputerDto>() 
            : new ObservableCollection<ComputerDto>(result);

    }
}
