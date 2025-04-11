using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Helpers;
using EquipmentInventory.Classes.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Data;

public static class ApiClient
{
    public static async Task<string> GetJwtToken(string login, string password)
    {
        var response = await ApiClientHelper.PostAsync<AuthRequest, JwtResponse>(
            "/api/Authorization/login",
            new AuthRequest { Login = login, Password = password },
            error => CustomMessageBoxHelper.Show("Ошибка входа", error)
        );

        if (response != null)
        {
            AuthorizationService.SetAuthorizationToken(response.Token);
        }

        return response?.Token;
    }

    

    public static async Task<bool> CheckAuthorization()
    {
        try
        {   
            var response = await App.ApiClient.GetAsync("/api/Authorization");
        
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return true;
            }

            AuthorizationService.ClearJwt();
            return false;
        }
        catch
        {
            return false;
        }
    }
}
