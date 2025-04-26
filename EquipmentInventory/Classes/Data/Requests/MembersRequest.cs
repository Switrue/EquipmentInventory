using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Data.Requests;

public static class MembersRequest
{
    public static async Task<List<MemberDto>> Get()
    {
        return await ApiClientHelper.GetAsync<List<MemberDto>>(
            "api/Members/get",
            Console.WriteLine);
    }

    public static async Task<ObservableCollection<MemberDto>> GetMembersItems()
    {
        var result = await Get();
        return result is null
            ? new ObservableCollection<MemberDto>()
            : new ObservableCollection<MemberDto>(result);
    }
}
