using EquipmentInventory.Classes.Data.Interfaces;

namespace EquipmentInventory.Classes.Data.ViewModels;

public class SettingsViewModel
{
    private IWindowService _windowService;
    private INavigationService _navigationService;

    public WindowManagementViewModel WindowManagement { get; }
    public SettingsListViewModel SettingsList { get; }

    public SettingsViewModel(IWindowService windowService, INavigationService navigationService)
    {
        _windowService = windowService;
        _navigationService = navigationService;

        WindowManagement = new WindowManagementViewModel(_windowService);
        SettingsList = new SettingsListViewModel(_navigationService);
    }
}
