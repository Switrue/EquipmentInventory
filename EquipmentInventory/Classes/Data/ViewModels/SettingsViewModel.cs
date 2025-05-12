using EquipmentInventory.Classes.Data.Interfaces;

namespace EquipmentInventory.Classes.Data.ViewModels;

public class SettingsViewModel
{
    public WindowManagementViewModel WindowManagement { get; }
    public SettingsListViewModel SettingsList { get; }

    public SettingsViewModel(IWindowService windowService, INavigationService navigationService)
    {
        WindowManagement = new WindowManagementViewModel(windowService);
        SettingsList = new SettingsListViewModel(navigationService);
    }
}
