using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Helpers;
using EquipmentInventory.Properties;
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
        return await ApiClientHelper.PatchAsync<RegisterRequest, BaseResponse>(
            "/api/Users/update-me",
            userUpdate,
            error => CustomMessageBoxHelper.Show(Strings.Error, error)
        );
    }

    public static async Task<BaseResponse> GetUserImageString()
    {
        return await ApiClientHelper.GetAsync<BaseResponse>(
            "/api/Users/image-me",
            error => CustomMessageBoxHelper.Show(Strings.Error, error)
        );
    }
}
