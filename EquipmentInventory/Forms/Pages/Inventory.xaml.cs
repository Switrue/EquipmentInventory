using EquipmentInventory.Classes.Helper;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace EquipmentInventory.Forms.Pages
{
    /// <summary>
    /// Логика взаимодействия для Inventory.xaml
    /// </summary>
    public partial class Inventory : Page
    {

        public Inventory()
        {
            InitializeComponent();
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            toggleGridBtn.Focus();
        }

        private void toggleGridBtn_Click(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleButton;

            if (toggle != null)
            {
                if (toggle.IsChecked == true)
                {
                    DrawerHost.OpenDrawerCommand.Execute(null, null);
                }
                else
                {
                    DrawerHost.CloseDrawerCommand.Execute(null, null);
                }

            }
        }
    }
}
