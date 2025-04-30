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

public static class MembersRequest
{
    public static async Task<List<MemberDto>> Get()
    {
        return await ApiClientHelper.GetAsync<List<MemberDto>>(
            "/api/Members/get",
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

    public static async Task<BaseResponse> Delete(long id)
    {
        return await ApiClientHelper.DeleteAsync<BaseResponse>(
            $"/api/Members/delete/{id}",
            error => CustomMessageBoxHelper.Show(Strings.Error, error));
    }

    public static async Task<BaseResponse> Add(MemberUpdateDto model)
    {
        return await ApiClientHelper.PostAsync<MemberUpdateDto, BaseResponse>(
            "/api/Members/add",
            model,
            error => CustomMessageBoxHelper.Show(Strings.Error, error));
    }

    public static async Task<BaseResponse> Update(long id, MemberUpdateDto model)
    {
        var filterParams = QueryParamsHelper.ToQueryParams(model);

        var filterQueryString = string.Join("&", filterParams
            .Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));

        return await ApiClientHelper.PatchAsync<MemberUpdateDto, BaseResponse>(
            $"/api/Members/update/{id}?{filterQueryString}",
            model,
            error => CustomMessageBoxHelper.Show(Strings.Error, error));
    }
}
