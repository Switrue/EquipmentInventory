using EquipmentInventory.Classes.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.GridTables
{
    /// <summary>
    /// Логика взаимодействия для Inventory.xaml
    /// </summary>
    public partial class Inventory : Page
    {
        private IMainTableSwitcher _mainTableSwitcher;

        public Inventory(IMainTableSwitcher mainTableSwitcher)
        {
            InitializeComponent();
            _mainTableSwitcher = mainTableSwitcher;
        }

        private void Button_Click(object sender, RoutedEventArgs e) => _mainTableSwitcher.TriggerANotification("Inv");
    }
}
