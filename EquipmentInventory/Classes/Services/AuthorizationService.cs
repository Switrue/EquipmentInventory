using EquipmentInventory.Forms.Pages.Admin;
using EquipmentInventory.Forms.Windows;
using EquipmentInventory.Classes.Data.Enums;
using EquipmentInventory.Properties;

namespace EquipmentInventory.Classes.Services;

public static class AuthorizationService
{
    public static void Authorize(string jwt)
    {
        var user = ApiService.ExtractUserFromJwt(jwt);
        var window = new MainWindow(user);
        UserPanel panel;

        if (user.Role == "Бухгалтер")
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
