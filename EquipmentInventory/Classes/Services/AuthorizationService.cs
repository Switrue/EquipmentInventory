using EquipmentInventory.Forms.Pages.Admin;
using EquipmentInventory.Forms.Windows;
using EquipmentInventory.Classes.Data.Enums;
using EquipmentInventory.Properties;
using System.Threading.Tasks;
namespace EquipmentInventory.Classes.Services;

public static class AuthorizationService
{
    public static async Task Authorize(string jwt)
    {
        await SetUser(jwt);

        var window = new MainWindow();
        UserPanel panel;

        if (App.user.Role == "Бухгалтер")
        {
            panel = new UserPanel(window, TabType.Tables, false);
        }
        else
        {
            panel = new UserPanel(window, TabType.Tables, true);
        }
        
        window.ChangeControlPanelFrameContent(panel);
        window.Show();
    }

    public static async Task SetUser(string jwt)
    {
        App.SetUser(ApiService.ExtractUserFromJwt(jwt));
        App.SetUserImage(await ApiService.GetUserImage());
    }

    public static void SaveJwt(string jwt)
    {
        Settings.Default.UserToken = jwt;
        Settings.Default.Save();
    }

    public static void ClearJwt()
    {
        Settings.Default.UserToken = string.Empty;
        Settings.Default.Save();
    }

    public static void SetAuthorizationToken(string jwt)
    {
        App.SetAuthorizationToken(jwt);
    }
}
