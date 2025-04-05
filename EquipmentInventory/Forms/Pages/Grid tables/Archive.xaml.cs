using EquipmentInventory.Classes.Interfaces;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.GridTables;

/// <summary>
/// Логика взаимодействия для Archive.xaml
/// </summary>
public partial class Archive : UserControl
{
    IMainTableSwitcher _mainTableSwitcher;

    public Archive(IMainTableSwitcher mainTableSwitcher)
    {
        InitializeComponent();
        _mainTableSwitcher = mainTableSwitcher;
    }

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e) => _mainTableSwitcher.TriggerANotification("Arc");
}
