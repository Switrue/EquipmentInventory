using EquipmentInventory.Classes.Data.ViewModels;
using EquipmentInventory.Properties;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.GridTables;

/// <summary>
/// Логика взаимодействия для Inventory.xaml
/// </summary>
public partial class Inventory : UserControl
{
    public Inventory(TableSwitcherInventoryViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
