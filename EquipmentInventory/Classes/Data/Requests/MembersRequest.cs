using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

    public static async Task<ObservableCollection<MemberDto>> GetMembersItemsAsync()
    {
        var result = await Get();
        return result is null
            ? new ObservableCollection<MemberDto>()
            : new ObservableCollection<MemberDto>(result);
    }

    public static async Task<List<object>> GetMembersDataAsync()
    {
        var items = await Get();
        return items.Select(m => new
        {
            m.Id,
            Имя = m.Username,
            Фамилия = m.Surname,
            Должность = m.Position
        }).ToList<object>();
    }
}
