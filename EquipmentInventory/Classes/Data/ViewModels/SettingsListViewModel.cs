using EquipmentInventory.Classes.Data.Interfaces;
using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Forms.Pages.Settings_tabs;
using EquipmentInventory.Properties;
using System.Collections.ObjectModel;

namespace EquipmentInventory.Classes.Data.ViewModels;

public class SettingsListViewModel
{
    private readonly INavigationService _navigationService;
    private SettingItem _selectedItem;

    public ObservableCollection<SettingItem> Items { get; }
    public SettingItem SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;

            if (_selectedItem != null)
            {
                _navigationService.NavigateTo(_selectedItem.Page);
            }
        }
    }

    public SettingsListViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        Items = new ObservableCollection<SettingItem>()
        {
            new SettingItem(
                title: Strings.Codes,
                icon: "Barcode",
                page: new CodesTab(Strings.Codes)),

            new SettingItem(
                title: Strings.Tables,
                icon: "TableSearch",
                page: new TablesTab(Strings.Tables)),

            new SettingItem(
                title: Strings.Application,
                icon: "Application",
                page: new AppSettingsTab(Strings.Application))
        };

        if (Items.Count > 0)
        {
            SelectedItem = Items[0];
        }
    }
}
