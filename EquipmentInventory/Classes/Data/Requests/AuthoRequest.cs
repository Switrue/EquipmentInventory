using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Helpers;
using EquipmentInventory.Classes.Services;
using EquipmentInventory.Properties;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Data.Requests;

public static class AuthoRequest
{
    public static async Task<string> GetJwtToken(string login, string password)
    {
        var response = await ApiClientHelper.PostAsync<AuthRequest, JwtResponse>(
            "/api/Authorization/login",
            new AuthRequest { Login = login, Password = password },
            error => CustomMessageBoxHelper.Show(Strings.LoginError, error)
        );

        if (response != null)
        {
            AuthorizationService.SetAuthorizationToken(response.Token);
        }

        return response?.Token;
    }

    public static async Task<string> UpdateJwtToken()
    {
        var response = await ApiClientHelper.GetAsync<JwtResponse>(
            "/api/Authorization/refresh",
            error => CustomMessageBoxHelper.Show(Strings.Error, error)
        );

        if (response != null)
        {
            AuthorizationService.SetAuthorizationToken(response.Token);

            if (!string.IsNullOrWhiteSpace(Settings.Default.UserToken))
            {
                AuthorizationService.SaveJwt(response.Token);
            }
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
