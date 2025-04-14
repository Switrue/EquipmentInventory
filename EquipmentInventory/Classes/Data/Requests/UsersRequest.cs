using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Helpers;
using EquipmentInventory.Properties;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Data.Requests;

public static class UsersRequest
{
    public static async Task<BaseResponse> UserRegister(RegisterRequest user)
    {
        return await ApiClientHelper.PostAsync<RegisterRequest, BaseResponse>(
            "/api/Authorization/register",
            user,
            error => CustomMessageBoxHelper.Show(Strings.Error, error)
        );
    }

    public static async Task<BaseResponse> UpdateUserProfile(RegisterRequest userUpdate)
    {
        return await ApiClientHelper.PostAsync<RegisterRequest, BaseResponse>(
            "/api/Users/updateProfile",
            userUpdate,
            error => CustomMessageBoxHelper.Show(Strings.Error, error)
        );
    }

    public static async Task<Users> GetUser(long id)
    {
        return await ApiClientHelper.GetAsync<Users>(
            $"/api/Users/user?userId={id}",
            error => CustomMessageBoxHelper.Show(Strings.Error, error)
        );
    }

    public static async Task<List<Users>> GetUsers()
    {
        return await ApiClientHelper.GetAsync<List<Users>>(
            "/api/Users/users",
            error => CustomMessageBoxHelper.Show(Strings.Error, error)
        );
    }

    public static async Task<BaseResponse> GetUserImageString()
    {
        return await ApiClientHelper.GetAsync<BaseResponse>(
            "/api/Users/image",
            error => CustomMessageBoxHelper.Show(Strings.Error, error)
        );
    }
}
