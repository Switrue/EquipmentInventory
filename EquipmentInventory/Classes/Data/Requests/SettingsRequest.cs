
using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Helpers;
using EquipmentInventory.Properties;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Data.Requests;

public class SettingsRequest
{
    public static async Task<BaseResponse> UseCode(BaseRequest model)
    {
        return await ApiClientHelper.PostAsync<BaseRequest, BaseResponse>(
            "/api/Settings/code",
            model,
            error => CustomMessageBoxHelper.Show(Strings.Error, error)
        );
    }
}
