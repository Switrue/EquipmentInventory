using EquipmentInventory.Classes.Data.Interfaces;
using EquipmentInventory.Classes.Data.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.GridTables;

/// <summary>
/// Логика взаимодействия для Inventory.xaml
/// </summary>
public partial class Inventory : UserControl
{
    private IMainTableSwitcher _mainTableSwitcher;
    private TableSwitcherInventoryViewModel _viewModel;

    public Inventory(IMainTableSwitcher mainTableSwitcher, TableSwitcherInventoryViewModel viewModel)
    {
        InitializeComponent();
        _mainTableSwitcher = mainTableSwitcher;
        _viewModel = viewModel;

        DataContext = _viewModel;
    }

    private void Button_Click(object sender, RoutedEventArgs e) 
        => _mainTableSwitcher.TriggerANotification("Inv");
}
