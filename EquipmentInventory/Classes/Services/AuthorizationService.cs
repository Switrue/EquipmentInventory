using EquipmentInventory.Classes.Data;
using EquipmentInventory.Classes.Enums;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Forms.Pages.Admin;
using EquipmentInventory.Forms.Windows;
using EquipmentInventory.Properties;
using System.Collections.Generic;

namespace EquipmentInventory.Classes.Services;

public class AuthorizationService
{
    private UserData _user;
    private Dictionary<string, (TabType tabType, bool isAdmin)> userPanelParams;

    public AuthorizationService(UserData user)
    {
        _user = user;
        InitializeData();
    }

    private void InitializeData()
    {
        userPanelParams = new Dictionary<string, (TabType tabType, bool isAdmin)>
        {
            { "Системный администратор", (TabType.Tables, true) },
            { "Бухгалтер", (TabType.Tables, false) }
        };
    }

    public void InitializeMainWindow()
    {
        MainWindow mainWindow = new MainWindow(_user);
        var role = _user.UserRole;

        if (!string.IsNullOrEmpty(role))
        {
            if (userPanelParams.TryGetValue(role, out var parameters))
            {
                UserPanel userPanel = new UserPanel(mainWindow, parameters.tabType, parameters.isAdmin);
                mainWindow.ChangeControlPanelFrameContent(userPanel);
                mainWindow.SaveUserPanelObject(userPanel);
            }
            else
            {
                CustomMessageBoxHelper.Show(Strings.Error, Strings.RoleNotFound, false);
            }

            mainWindow.Show();
        }
    }
}
